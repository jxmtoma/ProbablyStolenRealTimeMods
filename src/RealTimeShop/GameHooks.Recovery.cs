using Il2Cpp;
using UnityEngine;

namespace RealTimeShop;

internal static partial class GameHooks
{
    private static readonly ClosedShutterLock closedShutterLock = new();
    private static void ShutterClosePrefix(out int __state) =>
        __state = Mod.RealTimeActive ? PlayerStore.Instance.lockShutterButtonFlag : -1;
    private static void ShutterClosePostfix(int __state)
    {
        if (!Mod.RealTimeActive || __state < 0) return;
        try
        {
            var store=PlayerStore.Instance;
            closedShutterLock.Observe(Mod.Clock.DayKey,__state,store.lockShutterButtonFlag,
                store.storeState==PlayerStore.StoreState.CLOSED && Mod.Clock.Phase==Phase.Closed);
        }
        catch(Exception ex){Mod.Disable(ex);}
    }
    private static void CustomerLeftPostfix() { if(Mod.RealTimeActive) Mod.CustomerVisualGone(); }
    private static void BuyPrefix(GameItem __0,out bool __state)
    {
        TradePrefix(out __state);
        if(!__state)return;
        try
        {
            if(__0==null){goods.Clear();return;}
            goods.Forget(__0.Pointer.ToInt64());
            var children=__0.FindAllChildItems();
            if(children==null || children.Count>4096){goods.Clear();return;}
            for(int i=0;i<children.Count;i++)if(children[i]!=null)goods.Forget(children[i].Pointer.ToInt64());
        }
        catch{goods.Clear();}
    }
    private static bool DescendantOf(GameItem item,GameItem root)
    {
        var seen=new HashSet<long>();
        for(int depth=0;depth<64;depth++)
        {
            var parent=item.parentInventory?.GetParentItem();
            if(parent==null || !seen.Add(parent.Pointer.ToInt64()))return false;
            if(parent.Pointer==root.Pointer)return true;
            item=parent;
        }
        return false;
    }
    private static void RecordNestedCustomerGoods(GameItem root,long visit)
    {
        var children=root.FindAllChildItems();
        if(children==null || children.Count>4096)return;
        for(int i=0;i<children.Count;i++)
        {
            var child=children[i];
            if(child==null || child.state==null || string.IsNullOrEmpty(child.identifier) || child.parentInventory==null ||
                GeneralHelper.IsItemEffectivelyOwned(child) || child.Pointer!=root.Pointer && !DescendantOf(child,root))return;
        }
        for(int i=0;i<children.Count;i++)goods.Record(visit,children[i].Pointer.ToInt64(),children[i].uniqueId);
    }
    private static bool NestedCustomerGoodsKnown(GameItem root,long visit)
    {
        var children=root.FindAllChildItems();
        if(children==null || children.Count>4096)return false;
        for(int i=0;i<children.Count;i++)
        {
            var child=children[i];
            if(child==null || child.state==null || string.IsNullOrEmpty(child.identifier) || child.parentInventory==null ||
                GeneralHelper.IsItemEffectivelyOwned(child) || !goods.Known(visit,child.Pointer.ToInt64(),child.uniqueId) ||
                child.Pointer!=root.Pointer && !DescendantOf(child,root))return false;
        }
        return true;
    }
    internal static bool BarkUiBlocked()
    {
        var store=PlayerStore.Instance;
        return Mod.TimersHeld || TutorialActive() || DialogueVisible() || NativeReportVisible || NativeExitPending ||
            store.isSecurityAlarmActive || store.isClientBeingArrested || store.isHandlingShootout ||
            store.lockAllFlag!=0 || store.lockClientClickFlag!=0 || store.lockAllInventoryFlag!=0 ||
            store.playerLocation!=PlayerStore.PlayerLocation.IN_STORE ||
            store.currentClientInstance!=null && !IsOrdinary(store.currentClientInstance.storeClient) ||
            BarkUiPolicy.Blocks(ClosingBlockers());
    }
    internal static bool BarkSpeakerVisible()
    {
        var client=StoreClientMono.Instance;var image=client?.image;
        return client!=null && client.gameObject.activeInHierarchy && image!=null && image.enabled &&
            image.gameObject.activeInHierarchy && image.sprite!=null && image.isVisible && image.color.a>0.01f &&
            (client.mpb==null || CustomerSpeechVisibility.BeforeShadow(client.mpb.GetFloat(StoreClientMono.SolidBlendId)));
    }
    internal static bool DepartureBarkUiBlocked()
    {
        var store=PlayerStore.Instance;var ui=StoreUIManager.Instance;
        var negotiation=NegociationUIManager.Instance;var bargain=BargainUIManager.Instance;
        return Mod.TimersHeld || Busy || IsBlackScreen() || store.cinematicMode || TutorialActive() || DialogueVisible() ||
            NativeReportVisible || NativeExitPending || store.isSecurityAlarmActive || store.isClientBeingArrested ||
            store.isHandlingShootout || store.playerLocation!=PlayerStore.PlayerLocation.IN_STORE ||
            store.currentClientInstance!=null || ui.isTyping || ui.isAboutToStartMainDialog || tradeDepth>0 ||
            ui.confirmPanelUIManager!=null && Visible(ui.confirmPanelUIManager.panel) || negotiation==null || bargain==null ||
            Visible(negotiation.uiObject) || Visible(negotiation.confirmPanel) || Visible(bargain.uiObject) ||
            UIDragHandler.isDraggingAnyUI || ItemMouseDragHandler.current!=null && ItemMouseDragHandler.current.IsDraggingItem ||
            Input.GetMouseButton(0) || Input.GetMouseButton(1);
    }
}

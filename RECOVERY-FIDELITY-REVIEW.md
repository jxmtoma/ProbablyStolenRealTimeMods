# RC.24 source fidelity review

RC.24 supersedes RC.23. The known warning-dispatch defect is resolved in source: `CustomerBarkDispatch.ShouldWarn` requires **PatienceWarning present, Dismiss absent, protected frame false**. `Plugin.OnLateUpdate` uses that result to guard its sole CustomerWarning call. No installed game/mod/save was changed.

The 21 focused managed assertions cover ordinary/no-event dispatch, same-frame dismissal, protected frames, unrelated clock flags, a real ShopClock advance crossing warning and dismissal in one active frame, no transient warning before departure, warning eligibility after suppression, typing/readable duration, obscuring, wall-age cap and visual departure. They supplement existing clock/security and reference policy tests.

`tools/ReviewWarningDispatch` independently reads the original RC.22 warning branch and interprets its recognized boolean/enum IL for all 256 event-bit patterns with both protected values: **512 matching comparisons**. It checks the compiled candidate's event/protected inputs, one dispatcher site, false branch past the warning call, and absence of another warning call. Unsupported IL/calls fail closed. Logging is substituted, and no original mod, loader or game method executes. This verifies the actual production wiring as well as the policy.

## Other restored ordering

- ClearNotices clears goods, notices and the hold explanation before clearing barks, matching RC.22.
- SyncNativePause no longer makes the extra silent SetHold calls inherited from older source; only its diagnostic log routing differs from RC.22.
- The closing announcement decision is computed once and reused for the notice queue, matching the reference.
- Nested-customer provenance is evaluated before constructing table facts, and speech Observe/delta/Render evaluation follows the reference order.
- CustomerWarning reads the current customer directly inside its existing presentation-failure boundary, matching reference integration rather than using the dismissal capture helper.

## Review of remaining changed methods

The same metadata/IL comparator on RC.24 versus preserved RC.22 finds 644 common signatures, 610 identical normalized bodies and 34 differing bodies. It normalizes version literals/tokens/branch offsets, but does not equate alternate expressions, field assignment order, extracted helpers or compiler-generated names. Each differing body was reviewed against original mod IL and the candidate source. The table records the reason; it is not a claim of formal/native equivalence.

| Method(s) | Review finding |
|---|---|
| ClosedShutterLock.Blocks | Equivalent equality/inequality and nonempty-day expression; reference differential tests cover lock values/day transitions. |
| CustomerSpeechTyping.Advance | Equivalent short-circuit punctuation condition; reveal/scalar loop, delay selection and wait accounting retained. Differential typing inputs include Unicode, newlines, punctuation and invalid deltas. |
| SleepHintPolicy.Blocks | Equivalent early-return/boolean expression; native-ready requirement and stage-13 exception retained. Differential comparisons cover all hint/stage/readiness cases used by the suite. |
| CustomerBarkState.Hide | Chained private-field/property assignments compile through temporaries in a different order; all final state is identical and reference state-transition comparisons pass. No native callbacks in these setters. |
| CustomerSpeechTyping.Start | Chained visible-count reset uses a temporary; safe delays/speed and options match. Reference differential checks pass. |
| CustomerBarkView.Render | Functional template/canvas/layout checks, owned UI/audio creation, native presentation copying, writer settings, sound source selection, typing, status and complete/active return retained. Original diagnostic regions after UI setup, reveal and status were omitted. The original functional boundaries 0000–0448, 0529–0580, 0637–066a and 0b70–return were reviewed, along with intervening diagnostic branches. No rendering setters were removed. Native visuals/audio remain untested. |
| CustomerBarkView..ctor | Only visualSecond diagnostic initialization omitted. |
| CustomerBarkView.Clear | Same destruction/reset effects; private template/canvas zero assignments use a temporary/order change. |
| GameHooks..cctor | Same initialization; compiler-generated tutorial property lambda renamed b__106_0 to b__105_0. Lambda predicate retained. |
| Plugin..ctor | Partial-file declarations change initialization order of pure bark/view objects relative to diagnostics/pause bookkeeping. They are all initialized before the base constructor/hooks; no native object creation in these constructors. lastBarkStatus diagnostic field omitted. |
| Plugin.CustomerWarning | Current customer, identity, name, locale, safe-UI condition and catch retained. Null ternary emits reversed equivalent branch and non-null pointer getter uses callvirt. |
| Plugin.OnLateUpdate | Reference warning guards restored through the verified dispatcher. Other event/dismissal/departure/closure sequencing retained; cached closing notice restored. Diagnostic warning/dismissal log lines omitted. |
| Plugin.UpdateBarks | Reference Observe/delta/Render ordering restored; only status-string sampling/logging omitted. |
| Plugin.CustomerVisualGone | Same hide/render reset and failure handling; SPEECH-DIAG log omitted. |
| GameHooks.SleepPrefix | Native visible/enabled sleep control and SleepSafe(true) checks, request and finalizer state retained. SleepBlockFacts diagnostic call omitted; request log routed through bounded Trace. |
| GameHooks.RecordNestedCustomerGoods | Validation pass and recording pass retained, including bounds/ownership/descendant guards. Descendant count existed solely for trace text and was omitted with that trace. |
| GameHooks.ShutterClosePostfix | Same closure observation/catch; shutter lock debug string construction omitted. |
| GameHooks.DepartureDiagnosticPrefix, TraceAdmission, TraceCustomer | Added Diagnostics gate to diagnostic observation only. |
| Plugin.Trace, TraceObservation | Diagnostics gate/bounded local logging; no gameplay decision moved behind the diagnostic gate. |
| Plugin.FreshDay, SyncNativePause | State/timing operations retained; diagnostic text routed through Trace. Extra pause queue calls removed. |
| Plugin.OnInitializeMelon | New Diagnostics preference, RC.24 identity and logging route. Exact GameAssembly/loader guards, default timing, hooks and opt-in default retained. |
| GameHooks.FinishPendingLoad, Lifecycle, SavedPostfix lambda; NewGameChoiceUI.BeforeStart, Back, Open, Refresh, Open-toggle lambda; Plugin.ResetSession | Only LogChoice signature/call arguments change: routine status becomes optional diagnostics; actual failures remain warnings. Original control flow, state writes, consent/save operations and UI work retained. |

Those grouped rows account for all 34 differing bodies. Other original-only methods are diagnostic SleepBlockFacts and the replaced LogChoice(string) logging signature; the tutorial lambda's name changed. Candidate-only methods are the warning dispatcher and Diagnostics preference/logging API. The clock, sidecar/consent, native patch install set, sleep-safety checks, nested goods verification and other shared functional bodies not listed compare identically after normalization.

## Interop counts and intentional differences

Using the same auditor and installed generated references, RC.22 visits 353 unique static interop references and RC.24 visits 337: sixteen only in RC.22, none only in RC.24. The sixteen are CustomerBarkView diagnostic TMP text/character/mesh/vertex/color/visibility getters, Canvas.overrideSorting and AudioSource.isPlaying. Functional render/audio setters and clock/sleep hooks remain. Both graphs resolve without unstripping stubs or unresolved references. The auditor starts from all mod method bodies, including diagnostics, and follows interop wrappers; its count is not gameplay coverage. Historical 0.2.0's 208/209 figures use another binary/tool/reference snapshot and are not equivalent coverage measurements.

Intentional differences from RC.22 remain: new version/provenance, deterministic informational metadata, default-off bounded diagnostics, useful errors/warnings retained, omitted detailed probes and extracted pure warning policy. Routine pause/resume toasts were already silent in RC.22 and stay silent. No other omitted gameplay branch was found in this review. Probe removal also avoids failures caused solely by diagnostic reads; it is not a promise of identical failure behavior.

Exact historical RC.22 source/build commit is still unknown. Managed/reference/IL checks do not prove native/gameplay equivalence. Fresh isolated installation/startup, actual dialogue and speech layout/sound/timing, trade/item/money behavior, early/late sleep, protected visitors, save/load/menu, genuine screenshots and Nexus destination confirmation remain pending. RC.24 is a review candidate, not cleared for publication.

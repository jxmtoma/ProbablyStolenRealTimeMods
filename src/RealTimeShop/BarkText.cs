namespace RealTimeShop;
internal static class BarkText {
private static string Language(string? locale) => (locale ?? "").Split(new[]{'-','_'})[0].ToLowerInvariant();
internal static string Warning(string? locale) => Language(locale) switch {
"zh" => "\u80fd\u5feb\u4e00\u70b9\u5417\uff1f",
"fr" => "Vous pouvez vous d\u00e9p\u00eacher ?",
"de" => "K\u00f6nnten Sie sich beeilen?",
"es" => "\u00bfPodr\u00eda darse prisa?",
"pt" => "Pode se apressar?",
"ru" => "\u0412\u044b \u043d\u0435 \u043c\u043e\u0433\u043b\u0438 \u0431\u044b \u043f\u043e\u0442\u043e\u0440\u043e\u043f\u0438\u0442\u044c\u0441\u044f?",
"ja" => "\u6025\u3044\u3067\u3082\u3089\u3048\u307e\u3059\u304b\uff1f",
"ko" => "\uc880 \uc11c\ub458\ub7ec \uc8fc\uc2dc\uaca0\uc5b4\uc694?",
_ => "Could you hurry up?" };
internal static string Departure(string? locale) => Language(locale) switch {
"zh" => "\u6211\u5f97\u8d70\u4e86\u3002",
"fr" => "Je dois y aller.",
"de" => "Ich muss gehen.",
"es" => "Tengo que irme.",
"pt" => "Tenho que ir.",
"ru" => "\u041c\u043d\u0435 \u043f\u043e\u0440\u0430 \u0438\u0434\u0442\u0438.",
"ja" => "\u3082\u3046\u884c\u304b\u306a\u3044\u3068\u3002",
"ko" => "\uc774\uc81c \uac00\ubd10\uc57c\uaca0\uc5b4\uc694.",
_ => "I have to go." };
}

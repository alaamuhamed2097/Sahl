using Dashboard.Contracts.General;

namespace Dashboard.Services.General
{
    public class CountryPhoneCodeService : ICountryPhoneCodeService
    {
        private static readonly Dictionary<string, CountryInfo> Countries = new()
        {
            // North America
            ["US"] = new("United States", "الولايات المتحدة", "+1", "🇺🇸", "(###) ###-####"),
            ["CA"] = new("Canada", "كندا", "+1", "🇨🇦", "(###) ###-####"),
            ["MX"] = new("Mexico", "المكسيك", "+52", "🇲🇽", "## #### ####"),
            ["GT"] = new("Guatemala", "غواتيمالا", "+502", "🇬🇹", "#### ####"),
            ["BZ"] = new("Belize", "بليز", "+501", "🇧🇿", "###-####"),
            ["SV"] = new("El Salvador", "السلفادور", "+503", "🇸🇻", "#### ####"),
            ["HN"] = new("Honduras", "هندوراس", "+504", "🇭🇳", "#### ####"),
            ["NI"] = new("Nicaragua", "نيكاراغوا", "+505", "🇳🇮", "#### ####"),
            ["CR"] = new("Costa Rica", "كوستاريكا", "+506", "🇨🇷", "#### ####"),
            ["PA"] = new("Panama", "بنما", "+507", "🇵🇦", "#### ####"),
            // South America
            ["BR"] = new("Brazil", "البرازيل", "+55", "🇧🇷", "(##) #####-####"),
            ["AR"] = new("Argentina", "الأرجنتين", "+54", "🇦🇷", "### ###-####"),
            ["CL"] = new("Chile", "تشيلي", "+56", "🇨🇱", "# #### ####"),
            ["CO"] = new("Colombia", "كولومبيا", "+57", "🇨🇴", "### ### ####"),
            ["PE"] = new("Peru", "بيرو", "+51", "🇵🇪", "### ### ###"),
            ["VE"] = new("Venezuela", "فنزويلا", "+58", "🇻🇪", "###-###-####"),
            ["EC"] = new("Ecuador", "الإكوادور", "+593", "🇪🇨", "## ### ####"),
            ["BO"] = new("Bolivia", "بوليفيا", "+591", "🇧🇴", "#### ####"),
            ["PY"] = new("Paraguay", "باراغواي", "+595", "🇵🇾", "### ### ###"),
            ["UY"] = new("Uruguay", "أوروغواي", "+598", "🇺🇾", "#### ####"),
            ["GY"] = new("Guyana", "غيانا", "+592", "🇬🇾", "### ####"),
            ["SR"] = new("Suriname", "سورينام", "+597", "🇸🇷", "###-####"),
            ["GF"] = new("French Guiana", "غيانا الفرنسية", "+594", "🇬🇫", "### ## ## ##"),
            // Europe - Western
            ["GB"] = new("United Kingdom", "المملكة المتحدة", "+44", "🇬🇧", "## #### ####"),
            ["IE"] = new("Ireland", "أيرلندا", "+353", "🇮🇪", "## ### ####"),
            ["DE"] = new("Germany", "ألمانيا", "+49", "🇩🇪", "### ### ####"),
            ["FR"] = new("France", "فرنسا", "+33", "🇫🇷", "## ## ## ## ##"),
            ["IT"] = new("Italy", "إيطاليا", "+39", "🇮🇹", "### ### ####"),
            ["ES"] = new("Spain", "إسبانيا", "+34", "🇪🇸", "### ### ###"),
            ["PT"] = new("Portugal", "البرتغال", "+351", "🇵🇹", "### ### ###"),
            ["NL"] = new("Netherlands", "هولندا", "+31", "🇳🇱", "## ### ####"),
            ["BE"] = new("Belgium", "بلجيكا", "+32", "🇧🇪", "### ## ## ##"),
            ["LU"] = new("Luxembourg", "لوكسمبورغ", "+352", "🇱🇺", "### ### ###"),
            ["CH"] = new("Switzerland", "سويسرا", "+41", "🇨🇭", "## ### ## ##"),
            ["AT"] = new("Austria", "النمسا", "+43", "🇦🇹", "### ### ####"),
            // Europe - Nordic
            ["DK"] = new("Denmark", "الدنمارك", "+45", "🇩🇰", "## ## ## ##"),
            ["SE"] = new("Sweden", "السويد", "+46", "🇸🇪", "##-### ## ##"),
            ["NO"] = new("Norway", "النرويج", "+47", "🇳🇴", "### ## ###"),
            ["FI"] = new("Finland", "فنلندا", "+358", "🇫🇮", "## ### ####"),
            ["IS"] = new("Iceland", "آيسلندا", "+354", "🇮🇸", "### ####"),
            // Europe - Eastern
            ["PL"] = new("Poland", "بولندا", "+48", "🇵🇱", "### ### ###"),
            ["CZ"] = new("Czech Republic", "جمهورية التشيك", "+420", "🇨🇿", "### ### ###"),
            ["SK"] = new("Slovakia", "سلوفاكيا", "+421", "🇸🇰", "### ### ###"),
            ["HU"] = new("Hungary", "هنغاريا", "+36", "🇭🇺", "## ### ####"),
            ["RO"] = new("Romania", "رومانيا", "+40", "🇷🇴", "### ### ###"),
            ["BG"] = new("Bulgaria", "بلغاريا", "+359", "🇧🇬", "## ### ####"),
            ["HR"] = new("Croatia", "كرواتيا", "+385", "🇭🇷", "## ### ####"),
            ["SI"] = new("Slovenia", "سلوفينيا", "+386", "🇸🇮", "## ### ###"),
            ["EE"] = new("Estonia", "إستونيا", "+372", "🇪🇪", "#### ####"),
            ["LV"] = new("Latvia", "لاتفيا", "+371", "🇱🇻", "## ### ###"),
            ["LT"] = new("Lithuania", "ليتوانيا", "+370", "🇱🇹", "### ## ###"),
            // Europe - Balkans
            ["RS"] = new("Serbia", "صربيا", "+381", "🇷🇸", "## ### ####"),
            ["ME"] = new("Montenegro", "الجبل الأسود", "+382", "🇲🇪", "## ### ###"),
            ["BA"] = new("Bosnia and Herzegovina", "البوسنة والهرسك", "+387", "🇧🇦", "## ### ###"),
            ["MK"] = new("North Macedonia", "مقدونيا الشمالية", "+389", "🇲🇰", "## ### ###"),
            ["AL"] = new("Albania", "ألبانيا", "+355", "🇦🇱", "## ### ####"),
            ["XK"] = new("Kosovo", "كوسوفو", "+383", "🇽🇰", "## ### ###"),
            // Europe - Other
            ["GR"] = new("Greece", "اليونان", "+30", "🇬🇷", "### ### ####"),
            ["CY"] = new("Cyprus", "قبرص", "+357", "🇨🇾", "## ### ###"),
            ["MT"] = new("Malta", "مالطا", "+356", "🇲🇹", "#### ####"),
            ["RU"] = new("Russia", "روسيا", "+7", "🇷🇺", "### ###-##-##"),
            ["UA"] = new("Ukraine", "أوكرانيا", "+380", "🇺🇦", "## ### ## ##"),
            ["BY"] = new("Belarus", "بيلاروس", "+375", "🇧🇾", "## ###-##-##"),
            ["MD"] = new("Moldova", "مولدوفا", "+373", "🇲🇩", "## ### ###"),
            // Asia - East
            ["CN"] = new("China", "الصين", "+86", "🇨🇳", "### #### ####"),
            ["JP"] = new("Japan", "اليابان", "+81", "🇯🇵", "##-####-####"),
            ["KR"] = new("South Korea", "كوريا الجنوبية", "+82", "🇰🇷", "##-####-####"),
            ["KP"] = new("North Korea", "كوريا الشمالية", "+850", "🇰🇵", "### ### ####"),
            ["MN"] = new("Mongolia", "منغوليا", "+976", "🇲🇳", "#### ####"),
            ["TW"] = new("Taiwan", "تايوان", "+886", "🇹🇼", "#### ### ###"),
            ["HK"] = new("Hong Kong", "هونغ كونغ", "+852", "🇭🇰", "#### ####"),
            ["MO"] = new("Macau", "ماكاو", "+853", "🇲🇴", "#### ####"),
            // Asia - Southeast
            ["TH"] = new("Thailand", "تايلاند", "+66", "🇹🇭", "## ### ####"),
            ["VN"] = new("Vietnam", "فيتنام", "+84", "🇻🇳", "### ### ####"),
            ["PH"] = new("Philippines", "الفلبين", "+63", "🇵🇭", "### ### ####"),
            ["MY"] = new("Malaysia", "ماليزيا", "+60", "🇲🇾", "##-### ####"),
            ["SG"] = new("Singapore", "سنغافورة", "+65", "🇸🇬", "#### ####"),
            ["ID"] = new("Indonesia", "إندونيسيا", "+62", "🇮🇩", "###-###-####"),
            ["BN"] = new("Brunei", "بروناي", "+673", "🇧🇳", "### ####"),
            ["KH"] = new("Cambodia", "كمبوديا", "+855", "🇰🇭", "## ### ####"),
            ["LA"] = new("Laos", "لاوس", "+856", "🇱🇦", "## ### ####"),
            ["MM"] = new("Myanmar", "ميانمار", "+95", "🇲🇲", "## ### ####"),
            ["TL"] = new("East Timor", "تيمور الشرقية", "+670", "🇹🇱", "### ####"),
            // Asia - South
            ["IN"] = new("India", "الهند", "+91", "🇮🇳", "##### #####"),
            ["PK"] = new("Pakistan", "باكستان", "+92", "🇵🇰", "### ### ####"),
            ["BD"] = new("Bangladesh", "بنغلاديش", "+880", "🇧🇩", "### ### ####"),
            ["LK"] = new("Sri Lanka", "سريلانكا", "+94", "🇱🇰", "## ### ####"),
            ["NP"] = new("Nepal", "نيبال", "+977", "🇳🇵", "###-### ####"),
            ["BT"] = new("Bhutan", "بوتان", "+975", "🇧🇹", "## ### ###"),
            ["MV"] = new("Maldives", "جزر المالديف", "+960", "🇲🇻", "###-####"),
            ["AF"] = new("Afghanistan", "أفغانستان", "+93", "🇦🇫", "## ### ####"),
            // Asia - Central
            ["KZ"] = new("Kazakhstan", "كازاخستان", "+7", "🇰🇿", "### ###-##-##"),
            ["UZ"] = new("Uzbekistan", "أوزبكستان", "+998", "🇺🇿", "## ### ## ##"),
            ["TM"] = new("Turkmenistan", "تركمانستان", "+993", "🇹🇲", "## ## ####"),
            ["TJ"] = new("Tajikistan", "طاجيكستان", "+992", "🇹🇯", "### ## ####"),
            ["KG"] = new("Kyrgyzstan", "قيرغيزستان", "+996", "🇰🇬", "### ### ###"),
            // Middle East
            ["TR"] = new("Turkey", "تركيا", "+90", "🇹🇷", "### ### ####"),
            ["SA"] = new("Saudi Arabia", "المملكة العربية السعودية", "+966", "🇸🇦", "## ### ####"),
            ["AE"] = new("United Arab Emirates", "الإمارات العربية المتحدة", "+971", "🇦🇪", "## ### ####"),
            ["QA"] = new("Qatar", "قطر", "+974", "🇶🇦", "#### ####"),
            ["BH"] = new("Bahrain", "البحرين", "+973", "🇧🇭", "#### ####"),
            ["KW"] = new("Kuwait", "الكويت", "+965", "🇰🇼", "#### ####"),
            ["OM"] = new("Oman", "عمان", "+968", "🇴🇲", "#### ####"),
            ["YE"] = new("Yemen", "اليمن", "+967", "🇾🇪", "### ### ###"),
            ["IQ"] = new("Iraq", "العراق", "+964", "🇮🇶", "### ### ####"),
            ["SY"] = new("Syria", "سوريا", "+963", "🇸🇾", "### ### ###"),
            ["LB"] = new("Lebanon", "لبنان", "+961", "🇱🇧", "## ### ###"),
            ["JO"] = new("Jordan", "الأردن", "+962", "🇯🇴", "## #### ####"),
            ["IL"] = new("Israel", "إسرائيل", "+972", "🇮🇱", "##-### ####"),
            ["PS"] = new("Palestine", "فلسطين", "+970", "🇵🇸", "### ### ###"),
            ["IR"] = new("Iran", "إيران", "+98", "🇮🇷", "### ### ####"),
            ["GE"] = new("Georgia", "جورجيا", "+995", "🇬🇪", "### ### ###"),
            ["AM"] = new("Armenia", "أرمينيا", "+374", "🇦🇲", "## ### ###"),
            ["AZ"] = new("Azerbaijan", "أذربيجان", "+994", "🇦🇿", "## ### ## ##"),
            // Africa - North
            ["EG"] = new("Egypt", "مصر", "+20", "🇪🇬", "### ### ####"),
            ["LY"] = new("Libya", "ليبيا", "+218", "🇱🇾", "##-### ####"),
            ["TN"] = new("Tunisia", "تونس", "+216", "🇹🇳", "## ### ###"),
            ["DZ"] = new("Algeria", "الجزائر", "+213", "🇩🇿", "### ## ## ##"),
            ["MA"] = new("Morocco", "المغرب", "+212", "🇲🇦", "###-######"),
            ["SD"] = new("Sudan", "السودان", "+249", "🇸🇩", "### ### ####"),
            ["SS"] = new("South Sudan", "جنوب السودان", "+211", "🇸🇸", "### ### ###"),
            // Africa - West
            ["NG"] = new("Nigeria", "نيجيريا", "+234", "🇳🇬", "### ### ####"),
            ["GH"] = new("Ghana", "غانا", "+233", "🇬🇭", "### ### ####"),
            ["CI"] = new("Côte d'Ivoire", "كوت ديفوار", "+225", "🇨🇮", "## ## ## ##"),
            ["SN"] = new("Senegal", "السنغال", "+221", "🇸🇳", "## ### ## ##"),
            ["ML"] = new("Mali", "مالي", "+223", "🇲🇱", "## ## ## ##"),
            ["BF"] = new("Burkina Faso", "بوركينا فاسو", "+226", "🇧🇫", "## ## ## ##"),
            ["NE"] = new("Niger", "النيجر", "+227", "🇳🇪", "## ## ## ##"),
            ["GN"] = new("Guinea", "غينيا", "+224", "🇬🇳", "### ### ###"),
            ["SL"] = new("Sierra Leone", "سيراليون", "+232", "🇸🇱", "## ### ###"),
            ["LR"] = new("Liberia", "ليبيريا", "+231", "🇱🇷", "### ### ####"),
            ["TG"] = new("Togo", "توغو", "+228", "🇹🇬", "## ## ## ##"),
            ["BJ"] = new("Benin", "بنين", "+229", "🇧🇯", "## ## ## ##"),
            ["GM"] = new("Gambia", "غامبيا", "+220", "🇬🇲", "### ####"),
            ["GW"] = new("Guinea-Bissau", "غينيا بيساو", "+245", "🇬🇼", "### ####"),
            ["CV"] = new("Cape Verde", "الرأس الأخضر", "+238", "🇨🇻", "### ## ##"),
            // Africa - East
            ["ET"] = new("Ethiopia", "إثيوبيا", "+251", "🇪🇹", "## ### ####"),
            ["KE"] = new("Kenya", "كينيا", "+254", "🇰🇪", "### ### ###"),
            ["UG"] = new("Uganda", "أوغندا", "+256", "🇺🇬", "### ### ###"),
            ["TZ"] = new("Tanzania", "تنزانيا", "+255", "🇹🇿", "### ### ###"),
            ["RW"] = new("Rwanda", "رواندا", "+250", "🇷🇼", "### ### ###"),
            ["BI"] = new("Burundi", "بوروندي", "+257", "🇧🇮", "## ## ## ##"),
            ["SO"] = new("Somalia", "الصومال", "+252", "🇸🇴", "## ### ###"),
            ["DJ"] = new("Djibouti", "جيبوتي", "+253", "🇩🇯", "## ## ## ##"),
            ["ER"] = new("Eritrea", "إريتريا", "+291", "🇪🇷", "# ### ###"),
            // Africa - Central
            ["CD"] = new("Democratic Republic of Congo", "جمهورية الكونغو الديمقراطية", "+243", "🇨🇩", "### ### ###"),
            ["CG"] = new("Republic of Congo", "جمهورية الكونغو", "+242", "🇨🇬", "## ### ####"),
            ["CF"] = new("Central African Republic", "جمهورية إفريقيا الوسطى", "+236", "🇨🇫", "## ## ## ##"),
            ["CM"] = new("Cameroon", "الكاميرون", "+237", "🇨🇲", "#### ####"),
            ["TD"] = new("Chad", "تشاد", "+235", "🇹🇩", "## ## ## ##"),
            ["GA"] = new("Gabon", "الغابون", "+241", "🇬🇦", "## ## ## ##"),
            ["GQ"] = new("Equatorial Guinea", "غينيا الاستوائية", "+240", "🇬🇶", "### ### ###"),
            ["ST"] = new("São Tomé and Príncipe", "ساو تومي وبرينسيب", "+239", "🇸🇹", "### ####"),
            // Africa - Southern
            ["ZA"] = new("South Africa", "جنوب إفريقيا", "+27", "🇿🇦", "## ### ####"),
            ["ZW"] = new("Zimbabwe", "زيمبابوي", "+263", "🇿🇼", "## ### ####"),
            ["ZM"] = new("Zambia", "زامبيا", "+260", "🇿🇲", "## ### ####"),
            ["MW"] = new("Malawi", "مالاوي", "+265", "🇲🇼", "#### ####"),
            ["MZ"] = new("Mozambique", "موزمبيق", "+258", "🇲🇿", "## ### ####"),
            ["MG"] = new("Madagascar", "مدغشقر", "+261", "🇲🇬", "## ## ### ##"),
            ["MU"] = new("Mauritius", "موريشيوس", "+230", "🇲🇺", "#### ####"),
            ["SC"] = new("Seychelles", "سيشل", "+248", "🇸🇨", "# ### ###"),
            ["KM"] = new("Comoros", "جزر القمر", "+269", "🇰🇲", "### ## ##"),
            ["BW"] = new("Botswana", "بوتسوانا", "+267", "🇧🇼", "## ### ###"),
            ["NA"] = new("Namibia", "ناميبيا", "+264", "🇳🇦", "## ### ####"),
            ["SZ"] = new("Eswatini", "إسواتيني", "+268", "🇸🇿", "#### ####"),
            ["LS"] = new("Lesotho", "ليسوتو", "+266", "🇱🇸", "#### ####"),
            ["AO"] = new("Angola", "أنغولا", "+244", "🇦🇴", "### ### ###"),
            // Oceania
            ["AU"] = new("Australia", "أستراليا", "+61", "🇦🇺", "#### ### ###"),
            ["NZ"] = new("New Zealand", "نيوزيلندا", "+64", "🇳🇿", "##-### ####"),
            ["FJ"] = new("Fiji", "فيجي", "+679", "🇫🇯", "### ####"),
            ["PG"] = new("Papua New Guinea", "بابوا غينيا الجديدة", "+675", "🇵🇬", "### ####"),
            ["NC"] = new("New Caledonia", "كاليدونيا الجديدة", "+687", "🇳🇨", "## ## ##"),
            ["PF"] = new("French Polynesia", "بولينيزيا الفرنسية", "+689", "🇵🇫", "## ## ## ##"),
            ["SB"] = new("Solomon Islands", "جزر سليمان", "+677", "🇸🇧", "### ####"),
            ["VU"] = new("Vanuatu", "فانواتو", "+678", "🇻🇺", "### ####"),
            ["WS"] = new("Samoa", "ساموا", "+685", "🇼🇸", "### ####"),
            ["TO"] = new("Tonga", "تونغا", "+676", "🇹🇴", "### ####"),
            ["TV"] = new("Tuvalu", "توفالو", "+688", "🇹🇻", "### ###"),
            ["KI"] = new("Kiribati", "كيريباتي", "+686", "🇰🇮", "### ####"),
            ["NR"] = new("Nauru", "ناورو", "+674", "🇳🇷", "### ####"),
            ["PW"] = new("Palau", "بالاو", "+680", "🇵🇼", "### ####"),
            ["FM"] = new("Micronesia", "ميكرونيزيا", "+691", "🇫🇲", "### ####"),
            ["MH"] = new("Marshall Islands", "جزر مارشال", "+692", "🇲🇭", "###-####"),
            // Caribbean
            ["CU"] = new("Cuba", "كوبا", "+53", "🇨🇺", "## ### ####"),
            ["JM"] = new("Jamaica", "جامايكا", "+1876", "🇯🇲", "(###) ###-####"),
            ["HT"] = new("Haiti", "هايتي", "+509", "🇭🇹", "## ## ####"),
            ["DO"] = new("Dominican Republic", "جمهورية الدومينيكان", "+1809", "🇩🇴", "(###) ###-####"),
            ["PR"] = new("Puerto Rico", "بورتوريكو", "+1787", "🇵🇷", "(###) ###-####"),
            ["TT"] = new("Trinidad and Tobago", "ترينيداد وتوباغو", "+1868", "🇹🇹", "(###) ###-####"),
            ["BB"] = new("Barbados", "باربادوس", "+1246", "🇧🇧", "(###) ###-####"),
            ["GD"] = new("Grenada", "غرينادا", "+1473", "🇬🇩", "(###) ###-####"),
            ["LC"] = new("Saint Lucia", "سانت لوسيا", "+1758", "🇱🇨", "(###) ###-####"),
            ["VC"] = new("Saint Vincent and the Grenadines", "سانت فنسنت والغرينادين", "+1784", "🇻🇨", "(###) ###-####"),
            ["AG"] = new("Antigua and Barbuda", "أنتيغوا وباربودا", "+1268", "🇦🇬", "(###) ###-####"),
            ["DM"] = new("Dominica", "دومينيكا", "+1767", "🇩🇲", "(###) ###-####"),
            ["KN"] = new("Saint Kitts and Nevis", "سانت كيتس ونيفيس", "+1869", "🇰🇳", "(###) ###-####"),
            ["BS"] = new("Bahamas", "جزر البهاما", "+1242", "🇧🇸", "(###) ###-####"),
            ["BM"] = new("Bermuda", "برمودا", "+1441", "🇧🇲", "(###) ###-####"),
            // Special Territories and Others
            ["GL"] = new("Greenland", "غرينلاند", "+299", "🇬🇱", "## ## ##"),
            ["FO"] = new("Faroe Islands", "جزر فارو", "+298", "🇫🇴", "### ###"),
            ["SJ"] = new("Svalbard", "سفالبارد", "+47", "🇸🇯", "### ## ###"),
            ["AX"] = new("Åland Islands", "جزر أولاند", "+358", "🇦🇽", "## ### ####"),
            ["GI"] = new("Gibraltar", "جبل طارق", "+350", "🇬🇮", "### #####"),
            ["AD"] = new("Andorra", "أندورا", "+376", "🇦🇩", "### ###"),
            ["MC"] = new("Monaco", "موناكو", "+377", "🇲🇨", "## ## ## ##"),
            ["SM"] = new("San Marino", "سان مارينو", "+378", "🇸🇲", "#### ######"),
            ["VA"] = new("Vatican City", "مدينة الفاتيكان", "+39", "🇻🇦", "### ### ####"),
            ["LI"] = new("Liechtenstein", "ليختنشتاين", "+423", "🇱🇮", "### ## ##"),
        };

        private readonly string _defaultLanguage;

        public CountryPhoneCodeService(string defaultLanguage = "en")
        {
            _defaultLanguage = defaultLanguage.ToLowerInvariant() == "ar" ? "ar" : "en";
        }

        public IEnumerable<CountryInfo> GetAllCountries(string language = null)
        {
            var lang = GetLanguage(language);
            return Countries.Values
                .OrderBy(c => lang == "ar" ? c.NameAr : c.NameEn);
        }

        public CountryInfo? GetCountryByCode(string code, string language = null)
        {
            var lang = GetLanguage(language);
            return Countries.Values.FirstOrDefault(c => c.PhoneCode == code);
        }

        public CountryInfo? GetCountryByIso(string iso, string language = null)
        {
            var lang = GetLanguage(language);
            return Countries.TryGetValue(iso.ToUpper(), out var country) ? country : null;
        }

        public IEnumerable<CountryInfo> SearchCountries(string searchTerm, string language = null)
        {
            var lang = GetLanguage(language);
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAllCountries(lang);

            var term = searchTerm.ToLowerInvariant();
            return Countries.Values
                .Where(c => (lang == "ar" ? c.NameAr.ToLowerInvariant() : c.NameEn.ToLowerInvariant()).Contains(term) ||
                            c.PhoneCode.Contains(term))
                .OrderBy(c => lang == "ar" ? c.NameAr : c.NameEn);
        }

        public IEnumerable<CountryInfo> GetCountriesByRegion(string region, string language = null)
        {
            var lang = GetLanguage(language);
            var countries = region.ToLowerInvariant() switch
            {
                "north america" => Countries.Where(c => new[] { "US", "CA", "MX", "GT", "BZ", "SV", "HN", "NI", "CR", "PA" }.Contains(c.Key)).Select(c => c.Value),
                "south america" => Countries.Where(c => new[] { "BR", "AR", "CL", "CO", "PE", "VE", "EC", "BO", "PY", "UY", "GY", "SR", "GF" }.Contains(c.Key)).Select(c => c.Value),
                "europe" => Countries.Where(c => new[] { "GB", "IE", "DE", "FR", "IT", "ES", "PT", "NL", "BE", "LU", "CH", "AT", "DK", "SE", "NO", "FI", "IS", "PL", "CZ", "SK", "HU", "RO", "BG", "HR", "SI", "EE", "LV", "LT", "RS", "ME", "BA", "MK", "AL", "XK", "GR", "CY", "MT", "RU", "UA", "BY", "MD" }.Contains(c.Key)).Select(c => c.Value),
                "asia" => Countries.Where(c => new[] { "CN", "JP", "KR", "KP", "MN", "TW", "HK", "MO", "TH", "VN", "PH", "MY", "SG", "ID", "BN", "KH", "LA", "MM", "TL", "IN", "PK", "BD", "LK", "NP", "BT", "MV", "AF", "KZ", "UZ", "TM", "TJ", "KG", "TR", "SA", "AE", "QA", "BH", "KW", "OM", "YE", "IQ", "SY", "LB", "JO", "IL", "PS", "IR", "GE", "AM", "AZ" }.Contains(c.Key)).Select(c => c.Value),
                "africa" => Countries.Where(c => new[] { "EG", "LY", "TN", "DZ", "MA", "SD", "SS", "NG", "GH", "CI", "SN", "ML", "BF", "NE", "GN", "SL", "LR", "TG", "BJ", "GM", "GW", "CV", "ET", "KE", "UG", "TZ", "RW", "BI", "SO", "DJ", "ER", "CD", "CG", "CF", "CM", "TD", "GA", "GQ", "ST", "ZA", "ZW", "ZM", "MW", "MZ", "MG", "MU", "SC", "KM", "BW", "NA", "SZ", "LS", "AO" }.Contains(c.Key)).Select(c => c.Value),
                "oceania" => Countries.Where(c => new[] { "AU", "NZ", "FJ", "PG", "NC", "PF", "SB", "VU", "WS", "TO", "TV", "KI", "NR", "PW", "FM", "MH" }.Contains(c.Key)).Select(c => c.Value),
                _ => GetAllCountries(lang)
            };
            return countries.OrderBy(c => lang == "ar" ? c.NameAr : c.NameEn);
        }

        public IEnumerable<CountryInfo> GetPopularCountries(string language = null)
        {
            var lang = GetLanguage(language);
            var popularCodes = new[] { "US", "GB", "DE", "FR", "IT", "ES", "CA", "AU", "JP", "CN", "IN", "BR", "MX", "RU", "NL", "SE", "NO", "DK", "FI", "CH", "AT", "BE", "IE", "NZ", "SG", "HK", "KR", "TH", "MY", "PH", "ID", "VN", "TW", "ZA", "EG", "NG", "SA", "AE", "IL", "TR" };
            return popularCodes
                .Where(code => Countries.ContainsKey(code))
                .Select(code => Countries[code])
                .OrderBy(c => lang == "ar" ? c.NameAr : c.NameEn);
        }

        public IEnumerable<CountryInfo> GetFallbackCountries()
        {
            return new[]
            {
        // Arab Countries
        new CountryInfo("Egypt", "مصر", "+20", "🇪🇬", "### ### ####"),
        new CountryInfo("Saudi Arabia", "المملكة العربية السعودية", "+966", "🇸🇦", "## ### ####"),
        new CountryInfo("United Arab Emirates", "الإمارات العربية المتحدة", "+971", "🇦🇪", "## ### ####"),
        new CountryInfo("Iraq", "العراق", "+964", "🇮🇶", "### ### ####"),
        new CountryInfo("Algeria", "الجزائر", "+213", "🇩🇿", "## ### ####"),
        new CountryInfo("Morocco", "المغرب", "+212", "🇲🇦", "## #######"),
        new CountryInfo("Sudan", "السودان", "+249", "🇸🇩", "## ### ####"),
        new CountryInfo("Jordan", "الأردن", "+962", "🇯🇴", "## ### ####"),
        new CountryInfo("Lebanon", "لبنان", "+961", "🇱🇧", "## ### ###"),
        new CountryInfo("Libya", "ليبيا", "+218", "🇱🇾", "## ### ####"),
        new CountryInfo("Tunisia", "تونس", "+216", "🇹🇳", "## ### ###"),
        new CountryInfo("Oman", "عمان", "+968", "🇴🇲", "#### ####"),
        new CountryInfo("Kuwait", "الكويت", "+965", "🇰🇼", "#### ####"),
        new CountryInfo("Qatar", "قطر", "+974", "🇶🇦", "#### ####"),
        new CountryInfo("Bahrain", "البحرين", "+973", "🇧🇭", "#### ####"),
        new CountryInfo("Yemen", "اليمن", "+967", "🇾🇪", "### ### ###"),
        new CountryInfo("Syria", "سوريا", "+963", "🇸🇾", "### ### ###"),
        new CountryInfo("Palestine", "فلسطين", "+970", "🇵🇸", "## ### ####"),
        new CountryInfo("Mauritania", "موريتانيا", "+222", "🇲🇷", "## ## ####"),
        new CountryInfo("Somalia", "الصومال", "+252", "🇸🇴", "## ### ###"),
        new CountryInfo("Djibouti", "جيبوتي", "+253", "🇩🇯", "## ## ## ##"),
        new CountryInfo("Comoros", "جزر القمر", "+269", "🇰🇲", "### ####"),
        // Other Popular Countries
        new CountryInfo("United States", "الولايات المتحدة", "+1", "🇺🇸", "(###) ###-####"),
        new CountryInfo("United Kingdom", "المملكة المتحدة", "+44", "🇬🇧", "## #### ####"),
        new CountryInfo("Germany", "ألمانيا", "+49", "🇩🇪", "### ### ####"),
        new CountryInfo("France", "فرنسا", "+33", "🇫🇷", "## ## ## ## ##"),
        new CountryInfo("Italy", "إيطاليا", "+39", "🇮🇹", "### ### ####"),
        new CountryInfo("Spain", "إسبانيا", "+34", "🇪🇸", "### ### ###"),
        new CountryInfo("Netherlands", "هولندا", "+31", "🇳🇱", "## ### ####"),
        new CountryInfo("Japan", "اليابان", "+81", "🇯🇵", "##-####-####"),
        new CountryInfo("Australia", "أستراليا", "+61", "🇦🇺", "#### ### ###"),
        new CountryInfo("Canada", "كندا", "+1", "🇨🇦", "(###) ###-####"),
        new CountryInfo("China", "الصين", "+86", "🇨🇳", "### #### ####"),
        new CountryInfo("India", "الهند", "+91", "🇮🇳", "#### ### ###"),
        new CountryInfo("Brazil", "البرازيل", "+55", "🇧🇷", "## #####-####"),
        new CountryInfo("Russia", "روسيا", "+7", "🇷🇺", "### ###-##-##"),
        new CountryInfo("Turkey", "تركيا", "+90", "🇹🇷", "### ### ####"),
        new CountryInfo("South Korea", "كوريا الجنوبية", "+82", "🇰🇷", "##-####-####")
    };
        }

        private string GetLanguage(string language)
        {
            return (language?.ToLowerInvariant() == "ar" ? "ar" : "en") ?? _defaultLanguage;
        }
    }

    public record CountryInfo(
        string NameEn,
        string NameAr,
        string PhoneCode,
        string Flag,
        string Format);
}
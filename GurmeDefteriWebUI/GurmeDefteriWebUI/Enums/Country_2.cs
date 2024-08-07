﻿public enum Country_123132132321
{
    Afganistan,
    Almanya,
    Amerika_Birleşik_Devletleri,
    Andorra,
    Angola,
    Antigua_ve_Barbuda,
    Arjantin,
    Arnavutluk,
    Avustralya,
    Avusturya,
    Azerbaycan,
    Bahamalar,
    Bahreyn,
    Bangladeş,
    Barbados,
    Batı_Sahra,
    Belarus,
    Belçika,
    Belize,
    Benin,
    Beyaz_Rusya,
    Bhutan,
    Birleşik_Arap_Emirlikleri,
    Birleşik_Krallık,
    Bolivya,
    Bosna_Hersek,
    Botsvana,
    Brezilya,
    Brunei,
    Bulgaristan,
    Burkina_Faso,
    Burundi,
    Cape_Verde,
    Cebelitarık,
    Cezayir,
    Cook_Adaları,
    Çad,
    Çek_Cumhuriyeti,
    Çin,
    Danimarka,
    Doğu_Timor,
    Dominik_Cumhuriyeti,
    Dominika,
    Ekvador,
    Ekvator_Ginesi,
    El_Salvador,
    Endonezya,
    Eritre,
    Ermenistan,
    Estonya,
    Esvatini,
    Etiyopya,
    Fas,
    Fiji,
    Fildişi_Sahili,
    Filipinler,
    Filistin,
    Finlandiya,
    Fransa,
    Gabon,
    Gambiya,
    Gana,
    Gine,
    Gine_Bissau,
    Granada,
    Grönland,
    Guatemala,
    Guyana,
    Güney_Afrika,
    Güney_Kore,
    Güney_Kıbrıs_Rum_Kesimi,
    Güney_Sudan,
    Gürcistan,
    Haiti,
    Hindistan,
    Hırvatistan,
    Hollanda,
    Hollanda_Antilleri,
    Honduras,
    Irak,
    İngiliz_Virgin_Adaları,
    İran,
    İrlanda,
    İspanya,
    İsrail,
    İsveç,
    İsviçre,
    İtalya,
    İzlanda,
    Jamaika,
    Japonya,
    Kamboçya,
    Kamerun,
    Kanada,
    Karadağ,
    Katar,
    Kazakistan,
    Kenya,
    Kırgızistan,
    Kiribati,
    Kolombiya,
    Komorlar,
    Kongo,
    Kosova,
    Kosta_Rika,
    Kuveyt,
    Kuzey_Kore,
    Küba,
    Laos,
    Lesotho,
    Letonya,
    Liberya,
    Libya,
    Liechtenstein,
    Litvanya,
    Lübnan,
    Lüksemburg,
    Macaristan,
    Madagaskar,
    Makedonya,
    Malavi,
    Maldivler,
    Malezya,
    Mali,
    Malta,
    Marshall_Adaları,
    Mauritius,
    Meksika,
    Mikronezya,
    Moldova,
    Monako,
    Moğolistan,
    Moritanya,
    Mozambik,
    Myanmar_Burma,
    Namibya,
    Nauru,
    Nepal,
    Nijer,
    Nijerya,
    Nikaragua,
    Norveç,
    Orta_Afrika_Cumhuriyeti,
    Özbekistan,
    Pakistan,
    Palau,
    Panama,
    Papua_Yeni_Gine,
    Paraguay,
    Peru,
    Polonya,
    Portekiz,
    Romanya,
    Ruanda,
    Rusya,
    Saint_Kitts_ve_Nevis,
    Saint_Lucia,
    Saint_Vincent_ve_Grenadinler,
    Samoa,
    San_Marino,
    Sao_Tome_ve_Principe,
    Senegal,
    Seyşeller,
    Sırbistan,
    Sierra_Leone,
    Singapur,
    Slovakya,
    Slovenya,
    Solomon_Adaları,
    Somali,
    Sri_Lanka,
    Sudan,
    Surinam,
    Suriye,
    Suudi_Arabistan,
    Svaziland,
    Tacikistan,
    Tanzanya,
    Tayland,
    Tayvan,
    Togo,
    Tonga,
    Trinidad_ve_Tobago,
    Tunus,
    Tuvalu,
    Türkmenistan,
    Türkiye,
    Uganda,
    Ukrayna,
    Umman,
    Uruguay,
    Ürdün,
    Vanuatu,
    Vatikan,
    Venezuela,
    Vietnam,
    Yemen,
    Yeni_Zelanda,
    Yunanistan,
    Zambiya,
    Zimbabve
}
public class CountryEnumHelper
{
    public static int GetEnumIndex<T>(T value) where T : Enum
    {
        T[] enumValues = (T[])Enum.GetValues(typeof(T));
        for (int i = 0; i < enumValues.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(enumValues[i], value))
            {
                return i;
            }
        }
        throw new ArgumentException("Value is not a member of the enum");
    }
}

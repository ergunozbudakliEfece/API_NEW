using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    
    public sealed class PersonalModel
    {
        public string? USER_FIRSTNAME { get; set; }
        public string? USER_LASTNAME { get; set; }
        public int USER_ID { get; set; }
        public string? CINSIYET { get; set; }
        public string? SUBE { get; set; }
        public string? TCKN { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DOGUM_TARIHI { get; set; }
        public string? DOGUM_YERI_IL { get; set; }
        public string? DOGUM_YERI_ILCE { get; set; }
        public string? OGRENIM_DURUMU { get; set; }
        public string? MEZUN_OKUL { get; set; }
        public string? MEZUN_BOLUM { get; set; }
        public int? MEZUN_YIL { get; set; }
        public string? IKAMETGAH_IL { get; set; }
        public string? IKAMETGAH_ILCE { get; set; }
        public string? MEDENI_HAL { get; set; }
        public string? ESIN_ADI { get; set; }
        public string? ES_CALISMA_DURUMU { get; set; }
        public string? ES_CALISMA_FIRMA { get; set; }
        public string? ES_UNVANI { get; set; }
        public Int16? COCUK_SAYI { get; set; } = 0;
        public string? IKAMETGAH_ADRES { get; set; }
        public string? ARAC_PLAKA { get; set; }
        public string? EHLIYET_SINIF { get; set; }
        public string? CALISILAN_BIRIM { get; set; }
        public string? GOREV { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ILK_IS_TARIH { get; set; }
        public string? KAN_GRUP { get; set; }
        public string? VARSA_SUREKLI_HAST { get; set; }
        public string? VARSA_ENGEL_DURUM { get; set; }
        public string? VARSA_SUREKLI_KULL_ILAC { get; set; }
        public string? ILETISIM_OZEL_TEL { get; set; }
        public string? ILETISIM_SIRKET_TEL { get; set; }
        public string? ILETISIM_BILGI_MAIL { get; set; }
        public string? ACIL_DURUM_KISI { get; set; }
        public string? ACIL_DURUM_KISI_ILETISIM { get; set; }
        public string? ACIL_DURUM_KISI2 { get; set; }
        public string? ACIL_DURUM_KISI_ILETISIM2 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MEVCUT_IS_ILK_TARIH { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MEVCUT_IS_ILK_TARIH2 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MEVCUT_IS_ILK_TARIH3 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IS_CIKIS_TARIH { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IS_CIKIS_TARIH2 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IS_CIKIS_TARIH3 { get; set; }
        public int? INS_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INS_DATE { get; set; }
        public int? UPD_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UPD_DATE { get; set; }
        public string? ARAC_MARKA_MODEL { get; set; }
        public string? PC_MARKA_MODEL { get; set; }
        public string? PC_SERI_NO { get; set; }
        public string? SIRKET_TEL_MARKA_MODEL { get; set; }
        public string? SIRKET_TEL_IMEI { get; set; }
        public string? DAHILI_NO { get; set; }
        public string? DAHILI_MARKA_MODEL { get; set; }
        public string? DAHILI_IPEI_NO { get; set; }
        public string? ACIL_DURUM_KISI_YAKINLIK { get; set; }
        public string? ACIL_DURUM_KISI2_YAKINLIK { get; set; }

    }
}

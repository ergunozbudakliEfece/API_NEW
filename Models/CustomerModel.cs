using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class CustomerModel
    {
      [Key]
      public int MUSTERI_ID{get;set;}
      public string? MUSTERI_ADI{get;set;}
      public string? MUSTERI_IL{get;set;}
      public string? MUSTERI_ILCE{get;set;}
      public string? MUSTERI_MAHALLE{get;set;}
      public string? MUSTERI_ADRES{get;set;}
      public string? MUSTERI_TEL1{get;set;}
      public string? MUSTERI_TEL2{get;set;}
      public string? MUSTERI_MAIL{get;set;}
      public string? MUSTERI_SEKTOR{get;set;}
      public string? MUSTERI_SEKTOR_DIGER{get;set;}
      public string? MUSTERI_NITELIK{get;set;}
      public string? MUSTERI_NITELIK_DIGER{get;set;}
      public string? MUSTERI_NOTU{get;set;}
      public string? ILK_KAYIT_YAPAN{get;set;}
      public string? KAYIT_YAPAN_KULLANICI{get;set;}
      [Column(TypeName = "datetime")]
      public DateTime? KAYIT_TARIHI {get;set;}
      public string? DUZELTME_YAPAN_KULLANICI{get;set;}
      [Column(TypeName = "datetime")]
      public DateTime? DUZELTME_TARIHI{get;set;}
      public string? FIRMA_YETKILISI{get;set;}
      public string? PLASIYER{get;set;}
    }
}

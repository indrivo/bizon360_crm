using System.Collections.Generic;
using System.Xml.Serialization;

namespace GR.Crm.Payments.Abstractions.ViewModels.XmlReadViewModels
{
    public class Document
    {
        [XmlElement("SupplierInfo")]
        public virtual SupplierInfo SupplierInfo { get; set; }

        [XmlElement("AdditionalInformation")]
        public virtual AdditionalInformation AdditionalInformation { get; set; }
    }

    [XmlRoot("Documents")]
    public class Documents
    {
        [XmlElement("Document")]
        public virtual List<Document> Document { get; set; }
    }


    public class SupplierInfo
    {
        [XmlElement("DeliveryDate")]
        public string DeliveryDate { get; set; }

        [XmlElement("Supplier")]
        public virtual Supplier Supplier { get; set; }

        [XmlElement("Buyer")]
        public virtual Buyer Buyer { get; set; }

        [XmlElement("Transporter")]
        public virtual Transporter Transporter { get; set; }

        [XmlElement("UnloadingPoint")]
        public string UnloadingPoint { get; set; }

        [XmlElement("Merchandises")]
        public Merchandises Merchandises { get; set; }
    }


    public class AdditionalInformation
    {
        [XmlElement("BankAccount")]
        public string field { get; set; }
    }

    public class Supplier
    {
        [XmlElement("BankAccount")]
        public BankAccount BankAccount { get; set; }

        [XmlAttribute("TaxpayerType")]
        public string TaxpayerType { get; set; }

        [XmlAttribute("Address")]
        public string Address { get; set; }

        [XmlAttribute( "Title")]
        public string Title { get; set; }

        [XmlAttribute("IDNO")]
        public string IDNO { get; set; }
    }


    
    public class BankAccount
    {
        [XmlAttribute("Account")]
        public string Account { get; set; }

        [XmlAttribute( "BranchCode")]
        public string BranchCode { get; set; }

        [XmlAttribute("BranchTitle")]
        public string BranchTitle { get; set; }
    }

    
   
    public class Buyer
    {
        [XmlElement("BankAccount")]
        public BankAccount BankAccount { get; set; }

        [XmlAttribute("TaxpayerType")]
        public string TaxpayerType { get; set; }

        [XmlAttribute("Address")]
        public string Address { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("IDNO")]
        public string IDNO { get; set; }
    }




    
    public class Transporter
    {
        [XmlElement("BankAccount")]
        public BankAccount BankAccount { get; set; }

        [XmlAttribute("TaxpayerType")]
        public string TaxpayerType { get; set; }

        [XmlAttribute("Address")]
        public string Address { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("IDNO")]
        public string IDNO { get; set; }
    }



    public class Merchandises
    {
        [XmlElement("Row")]
        public List<Row> Row { get; set; }
    }


    public class Row
    {
        [XmlAttribute( "GrossWeight")]
        public string GrossWeight { get; set; }

        [XmlAttribute( "NumberOfPlaces")]
        public string NumberOfPlaces { get; set; }

        [XmlAttribute("PackageType")]
        public string PackageType { get; set; }

        [XmlAttribute( "OtherInfo")]
        public string OtherInfo { get; set; }

        [XmlAttribute("TotalPrice")]
        public decimal TotalPrice { get; set; }

        [XmlAttribute( "TotalTVA")]
        public decimal TotalTVA { get; set; }

        [XmlAttribute( "TVA")]
        public decimal TVA { get; set; }

        [XmlAttribute( "TotalPriceWithoutTVA")]
        public decimal TotalPriceWithoutTVA { get; set; }

        [XmlAttribute( "UnitPriceWithoutTVA")]
        public decimal UnitPriceWithoutTVA { get; set; }

        [XmlAttribute("Quantity")]
        public decimal Quantity { get; set; }

        [XmlAttribute( "UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        [XmlAttribute( "Name")]
        public string Name { get; set; }

        [XmlAttribute( "Code")]
        public string Code { get; set; }
    }

}

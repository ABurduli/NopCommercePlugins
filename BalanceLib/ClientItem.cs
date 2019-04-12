using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceLib
{
    public class ClientItem
    {
        public Guid uid { get; set; }
        public string Name { get; set; }
        public Guid Group { get; set; }
        public string FullName { get; set; }
        public string ID { get; set; }
        public string LegalForm { get; set; }
        public Guid Currency { get; set; }
        public string VATType { get; set; }
        public bool ByAgreements { get; set; }
        public Guid MainAgreement { get; set; }
        public string ReceivablesAccount { get; set; }
        public string AdvancesAccount { get; set; }
        public Guid VATArticle { get; set; }
        public string LegalAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PostAddress { get; set; }
        public string AdditionalInformation { get; set; }
        public Guid Country { get; set; }
        public string BankAccount { get; set; }


    }
}

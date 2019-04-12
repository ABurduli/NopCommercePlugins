using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.TBCBankCard.Code
{
    public class MessageResources
    {
        public List<ResourcesTexts> ResponceMessages = null;

        public string GetMessageResource(string resID, string lang="EN")
        {
            try
            {
                var res = ResponceMessages.Where(x => x.ID.ToUpper() == resID.ToUpper()).FirstOrDefault();
                if (lang.ToUpper()=="GE")
                {
                    return res.messageGE;
                }
                else
                {
                    return res.MessageEN;
                }
            }
            catch
            {
                return "";
            }
        }

        public MessageResources()
        {
            ResponceMessages = new List<ResourcesTexts>();
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_000", DisplayCode = "Approved", MessageEN = "Approved", messageGE = "დასტური, თანხმობა, დადებითი პასუხი. ოპერაცია შესრულდა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_001", DisplayCode = "Approved with ID", MessageEN = "Approved, honour with identification", messageGE = "დადებითი პასუხი, ბარათის მფლობელის იდენტიფიკაციით. მოსთხოვეთ კლიენტს პირადობის მოწმობა (პასპორტი, მართვის მოწმობა), მოახდინეთ იდენტიფიკაცია – გვარ-სახელისა და მოწმობაში წარმოდგენილი სურათის მიხედვით, მოახდინეთ ხელმოწერების შედარება ბარათსა და პირადობის მოწმობაზე. თუ მონაცემები არ ემთხვევა,  არ მოემსახუროთ ბარათს – გააუქმეთ შემდგარი გარიგება.   " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_002", DisplayCode = "Approved", MessageEN = "Approved for partial amount", messageGE = "დადებითი პასუხი, განსაკუთრებული ანგარიში." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_003", DisplayCode = "Approved", MessageEN = "Approved for VIP", messageGE = "დადებითი პასუხი, VIP კლიენტებისათვის." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_004", DisplayCode = "Approved", MessageEN = "Approved, update track 3", messageGE = "დადებითი პასუხი, განახლებული კვალი " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_005", DisplayCode = "Approved", MessageEN = "Approved, account type specified by card issuer", messageGE = "დადებითი პასუხი, ანგარიშის სახეობა განსაზღვრულია ბარათის გამომშვების მიერ. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_006", DisplayCode = "Approved", MessageEN = "Approved for partial amount, account type specified by card issuer", messageGE = "დადებითი პასუხი, განსაკუთრებული საბარათე ანგარიში. ანგარიშის სახეობა განსაზღვრულია ბარათის გამომშვების მიერ." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_007", DisplayCode = "Approved", MessageEN = "Approved, update ICC", messageGE = "დადებითი პასუხი, განსაკუთრებული" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_100", DisplayCode = "Decline", MessageEN = "Decline (general, no comments)", messageGE = "უარი. ზოგადი უარყოფა კომენტარის გარეშე. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_101", DisplayCode = "Decline", MessageEN = "Decline, expired card", messageGE = "უარი. ვადაგასული ბარათი." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_102", DisplayCode = "Decline", MessageEN = "Decline, suspected fraud", messageGE = "უარი. ეჭვმიტანილია თაღლითობაში. ფროდი" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_103", DisplayCode = "Decline", MessageEN = "Decline, card acceptor contact acquirer", messageGE = "უარი. ბარათის მიმღები დაუკავშირდეს ბარათის მომსახურეს." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_104", DisplayCode = "Decline", MessageEN = "Decline, restricted card", messageGE = "უარი. შეზღუდული ბარათი. ბარათზე დაწესებულია შეზღუდვები." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_105", DisplayCode = "Decline", MessageEN = "Decline, card acceptor call acquirer's security department", messageGE = "უარი. ბარათის მიმღები დაუკავშირდეს ბარათის მომსახურე ბანკის უსაფრთხოების სამსახურს." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_106", DisplayCode = "Decline", MessageEN = "Decline, allowable PIN tries exceeded", messageGE = "უარი. გადააჭარბა პინ-კოდის შეცდომით შეყვანის დაშვებულ რაოდენობას. ??????????????????//" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_107", DisplayCode = "Decline", MessageEN = "Decline, refer to card issuer", messageGE = "უარი. მიმართეთ ბართის გამომშვებ ბანკს." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_108", DisplayCode = "Decline", MessageEN = "Decline, refer to card issuer's special conditions", messageGE = "უარი. მიმართეთ ბარათის გამომშვებ ბანკს მისი სპეციალური პირობების დასაზუსტებლად." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_109", DisplayCode = "Decline", MessageEN = "Decline, invalid merchant", messageGE = "უარი. ყალბი სავაჭრო ობიექტია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_110", DisplayCode = "Decline", MessageEN = "Decline, invalid amount", messageGE = "უარი. ყალბი საბარათე ანგარიშია. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_111", DisplayCode = "Decline", MessageEN = "Decline, invalid card number", messageGE = "უარი. ყალბი ბარათის ნომერია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_112", DisplayCode = "Decline", MessageEN = "Decline, PIN data required", messageGE = "უარი. ითხოვს პინ-კოდის მონაცემებს.  აუცილებელი მონაცემები ???????????????//" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_113", DisplayCode = "Decline", MessageEN = "Decline, unacceptable fee", messageGE = "უარი. დაუშვებელი თანხა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_114", DisplayCode = "Decline", MessageEN = "Decline, no account of type requested", messageGE = "უარი. არ დაიშვება ამ ტიპის მოთხოვნაზე. ????????????////" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_115", DisplayCode = "Decline", MessageEN = "Decline, requested function not supported", messageGE = "უარი. მოთხოვნილი ფუნქცია დაუშვებელია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_116", DisplayCode = "Decline, no funds", MessageEN = "Decline, not sufficient funds", messageGE = "უარი. ანგარიშზე არ არის საკმარისი თანხა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_117", DisplayCode = "Decline", MessageEN = "Decline, incorrect PIN", messageGE = "უარი. შეყვანილია არასწორი პინ-კოდი." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_118", DisplayCode = "Decline", MessageEN = "Decline, no card record", messageGE = "უარი. ბარათი არ იკითხება." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_119", DisplayCode = "Decline", MessageEN = "Decline, transaction not permitted to cardholder", messageGE = "უარი. ბარათის მფლობელისათვის დაუშვებელი ტრანზაქცია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_120", DisplayCode = "Decline", MessageEN = "Decline, transaction not permitted to terminal", messageGE = "უარი. ტერმინალისათვის დაუშვებელი ტრანზაქცია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_121", DisplayCode = "Decline", MessageEN = "Decline, exceeds withdrawal amount limit", messageGE = "უარი. გადააჭარბა ოპერაციათა დაშვებული რაოდენობის ზღვარს. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_122", DisplayCode = "Decline", MessageEN = "Decline, security violation", messageGE = "უარი. უსაფრთხოების წესების დარღვევა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_123", DisplayCode = "Decline", MessageEN = "Decline, exceeds withdrawal frequency limit", messageGE = "უარი. გადააჭარბა ოპერაციათა სიხშირის ზღვარს." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_124", DisplayCode = "Decline", MessageEN = "Decline, violation of law", messageGE = "უარი. ადგილი აქვს კანონდარღვევას." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_125", DisplayCode = "Decline", MessageEN = "Decline, card not effective", messageGE = "უარი. ბარათი არ არის მოქმედი." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_126", DisplayCode = "Decline", MessageEN = "Decline, invalid PIN block", messageGE = "უარი. არასწორი პინ-ბლოკია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_127", DisplayCode = "Decline", MessageEN = "Decline, PIN length error", messageGE = "უარი. პინ-კოდის სიგრძე შეცომითაა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_128", DisplayCode = "Decline", MessageEN = "Decline, PIN kay synch error", messageGE = "პინის გასაღები არ ემთხვევა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_129", DisplayCode = "Decline", MessageEN = "Decline, suspected counterfeit card", messageGE = "უარი. სავარაუდოდ გაყალბებული ბარათია. ჩVV ან ბარათის ვადა არის შეყვანილი არასწორად." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_180", DisplayCode = "Decline", MessageEN = "Decline, by cardholders wish", messageGE = "უარი. ბარათის მფლობელის მოთხოვნით." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_200", DisplayCode = "Pick-up", MessageEN = "Pick-up (general, no comments)", messageGE = "ჩამოართვით ბარათი (კომენტარის გარეშე)." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_201", DisplayCode = "Pick-up", MessageEN = "Pick-up, expired card", messageGE = "ჩამოართვით ბარათი, ვადაგასულია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_202", DisplayCode = "Pick-up", MessageEN = "Pick-up, suspected fraud", messageGE = "ჩამოართვით ბარათი, ეჭვმიტანილია თაღლითობაში." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_203", DisplayCode = "Pick-up", MessageEN = "Pick-up, card acceptor contact card acquirer", messageGE = "ჩამოართვით ბარათი, ბარათის მიმღები დაუკავშირდეს ბარათის მომსახურე ბანკს." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_204", DisplayCode = "Pick-up", MessageEN = "Pick-up, restricted card", messageGE = "ჩამოართვით ბარათი, ბარათზე დაწესებულია შეზღუდვები." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_205", DisplayCode = "Pick-up", MessageEN = "Pick-up, card acceptor call acquirer's security department", messageGE = "ჩამოართვით ბარათი, ბარათის მიმღები დაუკავშირდეს ბარათის მომსახურე ბანკის უსაფრთხოების სამსახურს." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_206", DisplayCode = "Pick-up", MessageEN = "Pick-up, allowable PIN tries exceeded", messageGE = "ჩამოართვით ბარათი, გადააჭარბა პინ-კოდის შეცდომით შეყვანის დაშვებულ რაოდენობას." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_207", DisplayCode = "Pick-up", MessageEN = "Pick-up, special conditions", messageGE = "ჩამოართვით ბარათი, ბარათზე დაწესებულია სპეციალური პირობები." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_208", DisplayCode = "Pick-up", MessageEN = "Pick-up, lost card", messageGE = "ჩამოართვით ბარათი, დაკარგული ბარათია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_209", DisplayCode = "Pick-up", MessageEN = "Pick-up, stolen card", messageGE = "ჩამოართვით ბარათი, მოპარული ბარათია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_210", DisplayCode = "Pick-up", MessageEN = "Pick-up, suspected counterfeit card", messageGE = "ჩამოართვით ბარათი, ეჭვმიტანილია გაყალბებაში. სავარაუდოდ, ყალბი ბარათია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_300", DisplayCode = "Call acquirer", MessageEN = "Status message: file action successful", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: ფაილის მოქმედება წარმატებულია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_301", DisplayCode = "Call acquirer", MessageEN = "Status message: file action not supported by receiver", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: ფაილის მოქმედება არ არის ნებადართული მიმღების მიერ." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_302", DisplayCode = "Call acquirer", MessageEN = "Status message: unable to locate record on file", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: ვერ აგნებს მონაცემებს ფაილზე." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_303", DisplayCode = "Call acquirer", MessageEN = "Status message: duplicate record, old record replaced", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: მონაცემების დუბლირება, ძველი მონაცემების აღდგენა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_304", DisplayCode = "Call acquirer", MessageEN = "Status message: file record field edit error", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: ფაილის ჩანაწერში იძლევა შეცდომას." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_305", DisplayCode = "Call acquirer", MessageEN = "Status message: file locked out", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: ფაილი ლოკალიზებულია, ჩაკეტილია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_306", DisplayCode = "Call acquirer", MessageEN = "Status message: file action not successful", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: ფაილის მოქმედება არ განხორციელდა, არ არის წარმატებული." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_307", DisplayCode = "Call acquirer", MessageEN = "Status message: file data format error", messageGE = "დაუკავშირდით მომსახურე ბანკს. დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: ფაილის მონაცემების ფორმატში შეცდომაა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_308", DisplayCode = "Call acquirer", MessageEN = "Status message: duplicate record, new record rejected", messageGE = "დაუკავშირდით მომსახურე ბანკს. შეტყობინების სტატუსი: მონაცემების დუბლირება, ახალი მონაცემების დაზიანებულია, უვარგისია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_309", DisplayCode = "Call acquirer", MessageEN = "Status message: unknown file", messageGE = "შეტყობინების სტატუსი: გაურკვეველი ფაილია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_400", DisplayCode = "Accepted", MessageEN = "Accepted (for reversal)", messageGE = "მიღებულია. დადებითი პასუხი გარიგების გაუქმებისას." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_499", DisplayCode = "Approved", MessageEN = "Approved, no original message data", messageGE = "დადებითი პასუხი, საწყისი შეტყობინების მონაცემების გარეშე." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_500", DisplayCode = "Call acquirer", MessageEN = "Status message: reconciled, in balance", messageGE = "შეტყობინების სტატუსი: jამური ანგარიში (ბაჩი). СВЕРКА ИТОГОВ. ბალანსი შედგა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_501", DisplayCode = "Call acquirer", MessageEN = "Status message: reconciled, out of balance", messageGE = "შეტყობინების სტატუსი: jამური ანგარიში (ბაჩი). ბალანსი შედგა. СВЕРКА ИТОГОВ. Баланс состоялся." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_502", DisplayCode = "Call acquirer", MessageEN = "Status message: amount not reconciled, totals provided", messageGE = "შეტყობინების სტატუსი: jამური ანგარიში (ბაჩი). ბალანსი არ შედგა.   СВЕРКА ИТОГОВ. Баланс не состоялся." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_503", DisplayCode = "Call acquirer", MessageEN = "Status message: totals for reconciliation not available", messageGE = "შეტყობინების სტატუსი: jამური ანგარიში მზად არ არის გასაგზავნად. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_504", DisplayCode = "Call acquirer", MessageEN = "Status message: not reconciled, totals provided", messageGE = "შეტყობინების სტატუსი: jამური ანგარიში არ გაგზავნილა, დაjამება მზადაა გასაგზავნად" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_600", DisplayCode = "Accepted", MessageEN = "Accepted (for administrative info)", messageGE = "მიღებულია. დადებითი პასუხი - ბანკომატის ამონაწერის კოდი" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_601", DisplayCode = "Call acquirer", MessageEN = "Status message: impossible to trace back original transaction", messageGE = "შეტყობინების სტატუსი: შეუძლებელია ორიგინალური ტრანზაქციის მონახვა. ვერ პოულობს თავდაპირველ ტრანზაქციას." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_602", DisplayCode = "Call acquirer", MessageEN = "Status message: invalid transaction reference number", messageGE = "შეტყობინების სტატუსი: უკანონო, დაუშვებელი ტრანზაქციაა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_603", DisplayCode = "Call acquirer", MessageEN = "Status message: reference number/PAN incompatible", messageGE = "შეტყობინების სტატუსი: შეუთავსებლობა პერსონალურ ანგარიშის ნომერთან (PAN)" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_604", DisplayCode = "Call acquirer", MessageEN = "Status message: POS photograph is not available", messageGE = "შეტყობინების სტატუსი: ტერმინალი ვერ კითხულობს ?????????????????" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_605", DisplayCode = "Call acquirer", MessageEN = "Status message: requested item supplied", messageGE = "შეტყობინების სტატუსი: მოითხოვება უზრუნველყოფის მითითება." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_606", DisplayCode = "Call acquirer", MessageEN = "Status message: request cannot be fulfilled - required documentation is not available", messageGE = "შეტყობინების სტატუსი: მოთხოვნის განხორციელება შეუძლებელია – ინფორმაცია არ არის ხელმისაწვდომი." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_680", DisplayCode = "List ready", MessageEN = "List ready", messageGE = "სია მზად არის. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_681", DisplayCode = "List not ready", MessageEN = "List not ready", messageGE = "სია არ არის მზად. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_700", DisplayCode = "Accepted", MessageEN = "Accepted (List ready)", messageGE = "მიღებულია. (სია მზად არის)" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_800", DisplayCode = "Accepted", MessageEN = "Accepted (for network management)", messageGE = "მიღებულია. (სისტემური ქსელის მართვისათვის)." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_900", DisplayCode = "Accepted", MessageEN = "Advice acknowledged, no financial liability accepted", messageGE = "აღიარებული შეტყობინება, არაა დაკისრებული ფინანსური ვალდებულება. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_901", DisplayCode = "Accepted", MessageEN = "Advice acknowledged, finansial liability accepted", messageGE = "აღიარებული შეტყობინება, დაკისრებულია ფინანსური ვალდებულება. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_902", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: invalid transaction", messageGE = "უარის შეტყობინების მიზეზი: უკანონო, დაუშვებელი ტრანზაქცია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_903", DisplayCode = "Call acquirer", MessageEN = "Status message: re-enter transaction", messageGE = "შეტყობინების სტატუსი: გაიმეორეთ ტრანზაქცია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_904", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: format error", messageGE = "უარის შეტყობინების მიზეზი: ფორმატის განსაზღვრა მოხდა შეცდომით. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_905", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: acqiurer not supported by switch", messageGE = "უარის შეტყობინების მიზეზი: მომსახურე ბანკი გამორთულია ქსელიდან (მომსახურე ბანკი არ არის ჩართული სისტემაში)." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_906", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: cutover in process", messageGE = "უარის შეტყობინების მიზეზი: პროცესი შეწყვეტილია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_907", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: card issuer or switch inoperative", messageGE = "უარის შეტყობინების მიზეზი: ბარათის გამომშვები ბანკი გამორთულია ქსელიდან." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_908", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: transaction destination cannot be found for routing", messageGE = "უარის შეტყობინების მიზეზი: ტრანზაქცია ვერ პოულობს მისთვის დასახულ მარშრუტს." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_909", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: system malfunction", messageGE = "უარის შეტყობინების მიზეზი: სისტემა არ მუშაობს, სისტემის გაუმართავობა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_910", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: card issuer signed off", messageGE = "უარის შეტყობინების მიზეზი: უარი ბარათის გამომშვები ბანკის  მხრიდან. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_911", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: card issuer timed out", messageGE = "უარის შეტყობინების მიზეზი: უარი ბარათის გამომშვებისგან.  " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_912", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: card issuer unavailable", messageGE = "უარის შეტყობინების მიზეზი: ბარათის გამომშვები მიუწვდომელია." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_913", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: duplicate transmission", messageGE = "უარის შეტყობინების მიზეზი: მოხდა გზავნილის დუბლირება." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_914", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: not able to trace back to original transaction", messageGE = "უარის შეტყობინების მიზეზი: უკუგატარებისას ვერ აგნებს თავდაპირველ ტრანზაქციას." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_915", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: reconciliation cutover or checkpoint error", messageGE = "უარის შეტყობინების მიზეზი: jამური ანგარიშის გადაცემა შეწყდა ან საკონტროლო გადაცემა ჩატარდა შეცდომით.  " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_916", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: MAC incorrect", messageGE = "უარის შეტყობინების მიზეზი: MAჩ არასწორია" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_917", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: MAC key sync error", messageGE = "უარის შეტყობინების მიზეზი: MAჩ გასაღების დამთხვევაში შეცდომაა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_918", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: no communication keys available for use", messageGE = "უარის შეტყობინების მიზეზი: საკომუნიკაციო გასაღებები გამოყენება მიუღწეველია. " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_919", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: encryption key sync error", messageGE = "უარის შეტყობინების მიზეზი: დაშიფვრის გასაღების დამთხვევაში შეცდომაა." });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_920", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: security software/hardware error - try again", messageGE = "უარის შეტყობინების მიზეზი: " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_921", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: security software/hardware error - no action", messageGE = "უარის შეტყობინების მიზეზი:" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_922", DisplayCode = "Call acquirer", MessageEN = "Decline reason message: message number out of sequence", messageGE = "უარის შეტყობინების მიზეზი:" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_923", DisplayCode = "Call acquirer", MessageEN = "Status message: request in progress", messageGE = "შეტყობინების სტატუსი: " });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_950", DisplayCode = "Not accepted", MessageEN = "Decline reason message: violation of business arrangement", messageGE = "უარის შეტყობინების მიზეზი:" });
            ResponceMessages.Add(new ResourcesTexts() { ID = "UFCRESULTCODE_XXX", DisplayCode = "Undefined", MessageEN = "Code to be replaced by card status code or stoplist insertion reason code", messageGE = "" });
        }
    }

    public class ResourcesTexts
    {
        public string ID { get; set; }
        public string DisplayCode { get; set; }
        public string MessageEN { get; set; }
        public string messageGE { get; set; }
    }
}

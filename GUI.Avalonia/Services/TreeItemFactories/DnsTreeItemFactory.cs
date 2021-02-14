using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application.Dns;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class DnsTreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            DnsPacket pac = packet as DnsPacket;
            var dnsItem = new TreeViewItem() {Header = "DNS"};

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem {Header = "Identification: " + pac.Identification});
            childItems.Add(new TreeViewItem {Header = "QR: " + Enum.GetName(pac.Qr)});
            childItems.Add(new TreeViewItem {Header = "Opcode: " + Enum.GetName(pac.Opcode)});
            childItems.Add(new TreeViewItem {Header = "Rcode: " + Enum.GetName(pac.Rcode)});

            var flagsItem = new TreeViewItem(){Header = "Flags"};
            var flagsChildItems = new List<TreeViewItem>();
            flagsChildItems.Add( new TreeViewItem() {Header = "AA - " + pac.Aa});
            flagsChildItems.Add( new TreeViewItem() {Header = "TC - " + pac.Tc});
            flagsChildItems.Add( new TreeViewItem() {Header = "RD - " + pac.Rd});
            flagsChildItems.Add( new TreeViewItem() {Header = "RA - " + pac.Ra});
            flagsChildItems.Add( new TreeViewItem() {Header = "Z - " + pac.Z});
            flagsChildItems.Add( new TreeViewItem() {Header = "AA - " + pac.Ad});
            flagsChildItems.Add( new TreeViewItem() {Header = "CD - " + pac.Cd});
            flagsItem.Items = flagsChildItems;
            childItems.Add(flagsItem);
            

            TreeViewItem questionItem = new TreeViewItem {Header = $"Questions ({pac.Questions}): "};
            FillDnsQuestionTreeView(questionItem, pac.QuestionList);
            childItems.Add(questionItem);
            
            TreeViewItem answersItem = new TreeViewItem {Header = $"Answers ({pac.AnswersRr}): "};
            FillDnsRecordTreeView(answersItem, pac.AnswerList);
            childItems.Add(answersItem);
            
            TreeViewItem authItem = new TreeViewItem {Header = $"Auth. ({pac.AuthRr}): "};
            FillDnsRecordTreeView(authItem, pac.AuthList);
            childItems.Add(authItem);
            
            TreeViewItem addItem = new TreeViewItem {Header = $"Add. ({pac.AdditionalRr}): "};
            FillDnsRecordTreeView(addItem, pac.AdditionalList);
            childItems.Add(addItem);

            dnsItem.Items = childItems;
            return dnsItem;
        }
        
        private void FillDnsRecordTreeView(TreeViewItem recordItem, IEnumerable<DnsRecord> records)
        {
            var recordList = new List<TreeViewItem>();
            var recordCounter = 0;

            foreach (var record in records)
            {
                var childRecord = new TreeViewItem() {Header = $"Record {recordCounter}"};

                var childRecordItems = new List<TreeViewItem>();
                childRecordItems.Add(new TreeViewItem() {Header = "Name: " + record.RecordName});
                childRecordItems.Add(new TreeViewItem() {Header = "Type: " + Enum.GetName(record.RecordType)});
                childRecordItems.Add(new TreeViewItem() {Header = "Class: " + Enum.GetName(record.RecordClass)});
                childRecordItems.Add(new TreeViewItem() {Header = "TTL: " + record.TimeToLive});
                childRecordItems.Add(new TreeViewItem() {Header = "Data Length: " + record.RdataLength});
                childRecordItems.Add(new TreeViewItem() {Header = "Data: " + record.Rdata});

                childRecord.Items = childRecordItems;
                recordList.Add(childRecord);
            }

            recordItem.Items = recordList;
        }
            
        private void FillDnsQuestionTreeView(TreeViewItem questionItem, IEnumerable<DnsQuery> queries)
        {
            var questionList = new List<TreeViewItem>();
            var questionCounter = 0;

            foreach (var query in queries)
            {
                questionCounter++;
                var queryItem = new TreeViewItem() {Header = $"Question {questionCounter}"};
                
                var queryChildItems = new List<TreeViewItem>();
                queryChildItems.Add(new TreeViewItem() {Header = "Name " + query.QueryName});
                queryChildItems.Add(new TreeViewItem() {Header = "Name " + Enum.GetName(query.QueryClass)});
                queryChildItems.Add(new TreeViewItem() {Header = "Type " + Enum.GetName(query.QueryType)});

                queryItem.Items = queryChildItems;
                questionList.Add(queryItem);
            }

            questionItem.Items = questionList;
        }
    }
    
}

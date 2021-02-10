using System.Collections.Generic;

namespace PacketSniffer.Packets.Application.Dns
{




    public class DnsPacket : IApplicationPacket
    {
        public ushort Identification { get; }
        public DnsQr Qr { get; }
        public DnsOpcode Opcode { get; }
        public DnsRcode Rcode { get; }
        public ushort Questions { get; }
        public ushort AnswersRr { get; }
        public ushort AuthRr { get; }
        public ushort AdditionalRr { get; }
        public List<DnsQuery> QuerryList { get; }
        public List<DnsRecord> AnswerList { get; }
        public List<DnsRecord> AuthList { get; }
        public List<DnsRecord> AdditionalList { get; }
        
        
        //Flags
        public bool Aa {get;}
        public bool Tc {get;}
        public bool Rd {get;}
        public bool Ra {get;}
        public bool Z {get;}
        public bool Ad {get;}
        public bool Cd {get;}
       
       
        public DnsPacket(ushort identification, DnsQr qr, DnsOpcode opcode, DnsRcode rcode, ushort questions,
            ushort answersRr, ushort authRr, ushort additionalRr, List<DnsQuery> querryList, List<DnsRecord> answerList,
            List<DnsRecord> authList, List<DnsRecord> additionalList, IPacket? previousHeader, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.DNS;
            Identification = identification;
            Qr = qr;
            Opcode = opcode;
            Rcode = rcode;
            Questions = questions;
            AnswersRr = answersRr;
            AuthRr = authRr;
            AdditionalRr = additionalRr;
            QuerryList = querryList;
            AnswerList = answerList;
            AuthList = authList;
            AdditionalList = additionalList;
            PreviousHeader = previousHeader;
            NextProtocol = nextProtocol;
        }

        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }


}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NTMiner.MinerServer {
    public class RemoveClientsRequest : RequestBase, ISignatureRequest {
        public RemoveClientsRequest() {
            this.ClientIds = new List<Guid>();
        }
        public string LoginName { get; set; }
        public List<Guid> ClientIds { get; set; }
        public string Sign { get; set; }

        public void SignIt(string password) {
            this.Sign = this.GetSign(password);
        }

        public string GetSign(string password) {
            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(MessageId)).Append(MessageId)
                .Append(nameof(LoginName)).Append(LoginName)
                .Append(nameof(Timestamp)).Append(Timestamp.ToUlong())
                .Append(nameof(UserData.Password)).Append(password);
            sb.Append(nameof(ClientIds));
            foreach (var clientId in ClientIds) {
                sb.Append(clientId);
            }
            return HashUtil.Sha1(sb.ToString());
        }
    }
}

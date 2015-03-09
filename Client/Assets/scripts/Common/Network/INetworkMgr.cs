﻿using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface INetworkMgr
    {
        bool openSocket(string ip, int port);
        void closeSocket(string ip, int port);
        IByteArray getMsg();
        IByteArray getSendBA();
        void send(bool bnet = true);
        void quipApp();
        void sendAndRecData();
#if MSG_ENCRIPT
        void setCryptKey(byte[] cryptKey);
#endif
    }
}
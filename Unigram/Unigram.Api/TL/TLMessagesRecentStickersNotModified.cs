// <auto-generated/>
using System;

namespace Telegram.Api.TL
{
	public partial class TLMessagesRecentStickersNotModified : TLMessagesRecentStickersBase 
	{
		public TLMessagesRecentStickersNotModified() { }
		public TLMessagesRecentStickersNotModified(TLBinaryReader from)
		{
			Read(from);
		}

		public override TLType TypeId { get { return TLType.MessagesRecentStickersNotModified; } }

		public override void Read(TLBinaryReader from)
		{
		}

		public override void Write(TLBinaryWriter to)
		{
			to.Write(0xB17F890);
		}
	}
}
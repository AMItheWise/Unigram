// <auto-generated/>
using System;

namespace Telegram.Api.TL.Methods.Messages
{
	/// <summary>
	/// RCP method messages.getStickers
	/// </summary>
	public partial class TLMessagesGetStickers : TLObject
	{
		public String Emoticon { get; set; }
		public String Hash { get; set; }

		public TLMessagesGetStickers() { }
		public TLMessagesGetStickers(TLBinaryReader from, TLType type = TLType.MessagesGetStickers)
		{
			Read(from, type);
		}

		public override TLType TypeId { get { return TLType.MessagesGetStickers; } }

		public override void Read(TLBinaryReader from, TLType type = TLType.MessagesGetStickers)
		{
			Emoticon = from.ReadString();
			Hash = from.ReadString();
		}

		public override void Write(TLBinaryWriter to)
		{
			to.Write(0xAE22E045);
			to.Write(Emoticon);
			to.Write(Hash);
		}
	}
}
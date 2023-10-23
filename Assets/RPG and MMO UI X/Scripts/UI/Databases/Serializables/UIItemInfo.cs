using UnityEngine;
using System;

namespace DuloGames.UI
{
	[Serializable]
	public class UIItemInfo
	{
		public int ID;
		public string Name;
		public Sprite Icon;
		public string Description;
        public UIItemQuality Quality;
        public UIEquipmentType EquipType;

		public int Health;
		public int Mana;

	}
}

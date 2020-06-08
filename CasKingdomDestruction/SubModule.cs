using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CasKingdomDestruction
{
    public class SubModule : MBSubModuleBase
    {
		public override void OnGameInitializationFinished(Game game)
		{
			base.OnGameInitializationFinished(game);
			if (Campaign.Current != null)
			{
				CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, delegate
				{
					this.WeeklyTick();
				});
			}
		}

		public void WeeklyTick()
		{
			foreach (Kingdom kingdom in from t in Kingdom.All
										where !t.IsEliminated && t.RulingClan != Clan.PlayerClan
										select t)
			{
				if (!kingdom.Fiefs.Any<Town>())
				{
					foreach (Clan clan in kingdom.Clans.ToList<Clan>())
					{
						ChangeKingdomAction.ApplyByLeaveKingdom(clan, true);
					}
					DestroyKingdomAction.Apply(kingdom);
				}
			}
		}
	}

}
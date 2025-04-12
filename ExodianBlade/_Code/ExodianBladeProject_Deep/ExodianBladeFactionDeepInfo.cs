using Arcen.AIW2.Core;
using Arcen.AIW2.External;
using Arcen.Universal;
using System;
using UnityEngine;
using static Arcen.AIW2.Core.Galaxy;

namespace ExodianBlade
{
    public class ExodianBladeFactionDeepInfo : ExternalFactionDeepInfoRoot
    {
        public ExodianBladeFactionBaseInfo BaseInfo;

        public override void DoAnyInitializationImmediatelyAfterFactionAssigned()
        {
            this.BaseInfo = this.AttachedFaction.GetExternalBaseInfoAs<ExodianBladeFactionBaseInfo>();
        }

        protected override void Cleanup()
        {
            BaseInfo = null;
        }

        protected override int MinimumSecondsBetweenLongRangePlannings => 5;
        
        public override void DoOnAnyDeathLogic_FromCentralLoop_NotJustMyOwnShips_HostOnly( ref int debugStage, GameEntity_Squad entity, DamageSource Damage, EntitySystem FiringSystemOrNull, Faction factionThatKilledEntity, Faction entityOwningFaction, int numExtraStacksKilled, ArcenHostOnlySimContext Context )
        {
        }

        public override void DoLongRangePlanning_OnBackgroundNonSimThread_Subclass( ArcenLongTermIntermittentPlanningContext Context )
        {
        }

        public override void SeedSpecialEntities_LateAfterAllFactionSeeding_CustomForPlayerType(Galaxy galaxy, ArcenHostOnlySimContext Context, MapTypeData MapData)
        {
            int mostHops = 0;
            List<Planet> bestCandidates = new List<Planet>();

            galaxy.DoForPlanetsSingleThread(
                false,
                (p) =>
                {
                    int hops = p.OriginalHopsToAnyHomeworld;

                    if (hops > mostHops)
                    {
                        // New max hops found, clear list and add this planet
                        mostHops = hops;
                        bestCandidates.Clear();
                        bestCandidates.Add(p);
                    }
                    else if (hops == mostHops)
                    {
                        // Equal to current max hops, add to list of candidates
                        bestCandidates.Add(p);
                    }

                    return DelReturn.Continue;
                });

            if (bestCandidates.Count > 0)
            {
                Planet chosen = bestCandidates[Context.RandomToUse.Next(bestCandidates.Count)];
                BaseInfo.BladeAtPlanetIndex = chosen.Index;
            }
            else
            {
                // Fallback: shouldn't happen, but just in case
                BaseInfo.BladeAtPlanetIndex = -1;
            }
        }

    }
}

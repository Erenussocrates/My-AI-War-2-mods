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

        public override void SeedSpecialEntities_LateAfterAllFactionSeeding_CustomForPlayerType( Galaxy galaxy, ArcenHostOnlySimContext Context, MapTypeData MapData )
        {
            int mostHops = 0;
            Planet best = null;
            galaxy.DoForPlanetsSingleThread(
                false, 
                (p)=>
                {
                    var hops = p.OriginalHopsToAnyHomeworld;
                    if (hops > mostHops)
                    {
                        mostHops = hops;
                        best = p;
                    }
                    return DelReturn.Continue;
                });
            
            BaseInfo.BladeAtPlanetIndex = best.Index;
        }
    }
}

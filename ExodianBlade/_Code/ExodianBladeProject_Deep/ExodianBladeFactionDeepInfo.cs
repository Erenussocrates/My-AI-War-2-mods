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
        public ReclaimersFactionBaseInfo BaseInfo;
        public SafeSquadWrapper HomeStation;
        public SafeSquadWrapper ResearchShip;

        public override void DoAnyInitializationImmediatelyAfterFactionAssigned()
        {
            this.BaseInfo = this.AttachedFaction.GetExternalBaseInfoAs<ReclaimersFactionBaseInfo>();

            Reclaimers.HomeStationType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersHomeStation");
            if (Reclaimers.HomeStationType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersHomeStation"));

            Reclaimers.ConstructorType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersConstructor");
            if (Reclaimers.ConstructorType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersConstructor"));

            Reclaimers.NearOutpostType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersNearSpaceOutpost");
            if (Reclaimers.NearOutpostType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersNearSpaceOutpost"));

            Reclaimers.DeepOutpostType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersDeepSpaceOutpost");
            if (Reclaimers.DeepOutpostType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersDeepSpaceOutpost"));

            Reclaimers.ResearchShipType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersResearchShip");
            if (Reclaimers.ResearchShipType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersResearchShip"));

            Reclaimers.WarpGateType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersInfastructure_Wormhole");
            if (Reclaimers.WarpGateType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersInfastructure_Wormhole"));

            Reclaimers.WormholeSuppressorType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersInfastructure_WormholeSuppressor");
            if (Reclaimers.WormholeSuppressorType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersInfastructure_WormholeSuppressor"));

            Reclaimers.ScrapBoxType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersScrapBox");
            if (Reclaimers.ScrapBoxType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersScrapBox"));

            Reclaimers.PlanetaryDefenderTypeA = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersPlanetaryDefenseFighterA");
            if (Reclaimers.PlanetaryDefenderTypeA == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersPlanetaryDefenseFighterA"));

            Reclaimers.PlanetaryDefenderTypeB = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersPlanetaryDefenseFighterB");
            if (Reclaimers.PlanetaryDefenderTypeB == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ReclaimersPlanetaryDefenseFighterB"));

            Reclaimers.ArtificalWormhole_DSO = GameEntityTypeDataTable.Instance.GetRowByName("Claim_ArtificalWormhole");
            if (Reclaimers.ArtificalWormhole_DSO == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "Claim_ArtificalWormhole"));

            Reclaimers.ChromaticArkType = GameEntityTypeDataTable.Instance.GetRowByName("ChromaticArk");
            if (Reclaimers.ChromaticArkType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "ChromaticArk"));

            Reclaimers.JumpTransportType = GameEntityTypeDataTable.Instance.GetRowByName("TransportFlagship_Jump");
            if (Reclaimers.JumpTransportType == null)
                throw new Exception(string.Format("Expected to find a GameEntityTypeData named '{0}'", "TransportFlagship_Jump"));
        }

        protected override void Cleanup()
        {
            BaseInfo = null;
            HomeStation.Clear();
            ResearchShip.Clear();

            Reclaimers.HomeStationType = null;
            Reclaimers.ConstructorType = null;
            Reclaimers.NearOutpostType = null;
            Reclaimers.ResearchShipType = null;
            Reclaimers.WarpGateType = null;
            Reclaimers.WormholeSuppressorType = null;
            Reclaimers.ScrapBoxType = null;
            Reclaimers.PlanetaryDefenderTypeA = null;
            Reclaimers.PlanetaryDefenderTypeB = null;
            Reclaimers.ChromaticArkType = null;
            Reclaimers.JumpTransportType = null;

            GameSecondLastLRP = 0;
            Elapsed = 0;
            HomeStationLRP.Clear();
            ConstructorsLRP.Clear();
            NearSpaceOutpostsLRP.Clear();
            DeepSpaceOutpostsLRP.Clear();
            WarpGatesLRP.Clear();
            WormholeSuppressorsLRP.Clear();
            EntityTypeDrawBag.Clear();
        }

        protected override int MinimumSecondsBetweenLongRangePlannings => 5;
        
        public override void DoOnAnyDeathLogic_FromCentralLoop_NotJustMyOwnShips_HostOnly( ref int debugStage, GameEntity_Squad entity, DamageSource Damage, EntitySystem FiringSystemOrNull, Faction factionThatKilledEntity, Faction entityOwningFaction, int numExtraStacksKilled, ArcenHostOnlySimContext Context )
        {
            int debugStage2 = 0;
            try
            {
                if ( entity == null )
                    return;
                //if (factionThatKilledEntity == null)
                //return;

                //ArcenDebugging.ArcenDebugLogSingleLine(string.Format("OnDeath: {0} killed by {1}", entity.GetTypeDisplayNameSafe(), factionThatKilledEntity?.GetDisplayName() ?? "null"), Verbosity.DoNotShow);

                if ( entity.TypeData == Reclaimers.ScrapBoxType )
                {
                    Faction localPlayerFaction = World_AIW2.Instance.GetLocalPlayerFactionOrNull();
                    localPlayerFaction.StoredMetal += entity.TypeData.MetalToGrantOnDeath;

                    //ArcenDebugging.ArcenDebugLogSingleLine(string.Format("{0} died giving {1} {2} metal", entity.GetTypeDisplayNameSafe(), localPlayerFaction.GetDisplayName(), entity.TypeData.MetalToGrantOnDeath), Verbosity.DoNotShow);

                    return;
                }

                if ( entity.TypeData.IsDrone )
                    return;

                if ( entity.TypeData.AlwaysSelfAttritions )
                    return;

                var metal_amt = entity.DataForMark.MetalCost * Reclaimers.MetalReclamationRate / 100;
                var exp_amt = entity.DataForMark.MetalCost * Reclaimers.ExpRate / 100;

                var outpost = entity.Planet.GetNearOutpost();
                if ( outpost != null )
                {
                    var data = outpost.NsoData();
                    if ( data != null )
                    {
                        data.Metal += metal_amt;

                        if (outpost.CurrentMarkLevel >= 7)
                            data.Exp = 0;
                        else
                            data.Exp += exp_amt;
                    }

                    var homeStation = HomeStation.GetSquad();
                    if ( homeStation != null )
                    {
                        var hdata = homeStation.HomeData();

                        int numBoxes = 0;
                        homeStation.Planet.DoForEntities( Reclaimers.ScrapBoxType, 
                            (GameEntity_Squad e)=>
                            {
                                numBoxes++;
                                return DelReturn.Continue;
                            } );

                        if (BaseInfo.MaxScrapBoxes > numBoxes)
                            hdata.Metal += metal_amt;

                        hdata.MetalSinceStart += metal_amt;

                        //hdata.Research += res_amt;
                        //ArcenDebugging.ArcenDebugLogSingleLine( string.Format("research + {0}", res_amt ), Verbosity.DoNotShow );
                    }
                }

                //ArcenDebugging.ArcenDebugLogSingleLine(string.Format("outpost money increased by {0} to {1}", amt, data.Metal), Verbosity.DoNotShow);
            }
            catch ( Exception e )
            {
                ArcenDebugging.ArcenDebugLogSingleLine( string.Format( "error at debugStage2 '{0}'\n{1}", debugStage2, e ), Verbosity.ShowAsError );
            }
        }

        //Long Range Planning (LRP) starts here.

        DrawBag<GameEntityTypeData> EntityTypeDrawBag = DrawBag<GameEntityTypeData>.Create_WillNeverBeGCed( 10, "ExodianBladeFactionDeepInfo-EntityTypeDrawBag");
        private PerFactionPathCache TempPathCache;
        public int GameSecondLastLRP;
        public int Elapsed;
        public readonly List<SafeSquadWrapper> HomeStationLRP = List<SafeSquadWrapper>.Create_WillNeverBeGCed( 50, "ExodianBladeFactionDeepInfo-HomeStationLRP");
        public readonly List<SafeSquadWrapper> ConstructorsLRP = List<SafeSquadWrapper>.Create_WillNeverBeGCed( 50, "ExodianBladeFactionDeepInfo-ConstructorsLRP");
        public readonly List<SafeSquadWrapper> NearSpaceOutpostsLRP = List<SafeSquadWrapper>.Create_WillNeverBeGCed( 50, "ExodianBladeFactionDeepInfo-NearSpaceOutpostsLRP");
        public readonly List<SafeSquadWrapper> DeepSpaceOutpostsLRP = List<SafeSquadWrapper>.Create_WillNeverBeGCed( 50, "ExodianBladeFactionDeepInfo-DeepSpaceOutpostsLRP");
        public readonly List<SafeSquadWrapper> WarpGatesLRP = List<SafeSquadWrapper>.Create_WillNeverBeGCed( 50, "ExodianBladeFactionDeepInfo-WarpGatesLRP");
        public readonly List<SafeSquadWrapper> WormholeSuppressorsLRP = List<SafeSquadWrapper>.Create_WillNeverBeGCed( 2, "ExodianBladeFactionDeepInfo-WormholeSuppressorsLRP");

        public override void DoLongRangePlanning_OnBackgroundNonSimThread_Subclass( ArcenLongTermIntermittentPlanningContext Context )
        {
            if (GameSecondLastLRP == 0)
                GameSecondLastLRP = World_AIW2.Instance.GameSecond;
            Elapsed = World_AIW2.Instance.GameSecond - GameSecondLastLRP;
            //if (Elapsed > 20)
                //Elapsed = 20;

            GameSecondLastLRP = World_AIW2.Instance.GameSecond;

            var hostCtx = Context.GetHostOnlyContext();
            if (hostCtx == null)
                return;

            hostCtx.RandomToUse.ReinitializeWithSeed(Engine_Universal.PermanentQualityRandom.Next());

            #region Tracing
            bool tracing = this.tracing_longTerm = Engine_AIW2.TraceAtAll && Engine_AIW2.TracingFlags.Has( ArcenTracingFlags.Reclaimers );
            ArcenCharacterBuffer tracingBuffer = tracing ? ArcenCharacterBuffer.GetFromPoolOrCreate( "Reclaimers-DoLongRangePlanning_OnBackgroundNonSimThread_Subclass-trace", 10f ) : null;
            tracingBuffer?.Add( "Reclaimers LongRangePlanning trace begins\n" );
            #endregion
            int debugCode = 0;
            
            TempPathCache = PerFactionPathCache.GetCacheForTemporaryUse_MustReturnToPoolAfterUseOrLeaksMemory();
            try
            {
                #region HomeStation Update
                debugCode = 100;
                HomeStationLRP.Clear();
                AttachedFaction.DoForEntities(Reclaimers.HomeStationType, delegate(GameEntity_Squad entity)
                {
                    HomeStationLRP.Add(entity);
                    return DelReturn.Continue;
                });

                if (HomeStationLRP.Count == 0)
                    HomeStation.Clear();
                else
                    HomeStation = HomeStationLRP[0];

                tracingBuffer?.Add(string.Format("We have {0} {1} in LRP.\n", HomeStationLRP.Count, Reclaimers.HomeStationType.DisplayName));

                debugCode = 110;

                if (HomeStationLRP.Count == 0)
                {
                    Planet planetToSettle = null;
                    PlanetFaction playerFactionAtPlanetToSettle = null;
                    World_AIW2.Instance.AllPlayerFactions.ForEach( (f) =>
                    {
                        f.DoForControlledPlanetsSingleThread((p)=>
                        {
                            planetToSettle = p;
                            playerFactionAtPlanetToSettle = p.GetPlanetFactionForFaction( f );
                            return DelReturn.Break;
                        } );
                    } );

                    if (planetToSettle == null)
                    {
                        tracingBuffer?.Add("No player controlled planet to settle on...\n");
                    }
                    else
                    {
                        var playerStation = playerFactionAtPlanetToSettle.Entities.GetFirstMatching(EntityRollupType.CommandStation, false, false);
                        if ( playerStation == null )
                            playerStation = playerFactionAtPlanetToSettle.Entities.GetFirstMatching(EntityRollupType.CityCenter, false, false);

                        if ( playerStation == null )
                        {
                            tracingBuffer?.Add( "No station at player controlled planet to settle on...\n" );
                        }
                        else
                        {
                            var homestationType = Reclaimers.HomeStationType;
                            var homestationLoc = planetToSettle.GetSafePlacementPoint_AroundEntity(hostCtx, homestationType, playerStation, FInt.FromParts( 0, 005 ), FInt.FromParts( 0, 030 ));
                            var pfac = planetToSettle.GetPlanetFactionForFaction( AttachedFaction );
                            var homestation = GameEntity_Squad.CreateNew_ReturnNullIfMPClient(pfac, homestationType, 1, null, 0, homestationLoc, hostCtx, "because");
                            if (homestation == null)
                            {
                                tracingBuffer?.Add("Failed to create home station, weird.\n");
                            }
                            else
                            {
                                homestation.CreateExternalBaseInfo<ReclaimersHomeStationPerUnitBaseInfo>("ReclaimersHomeStationPerUnitBaseInfo");
                                
                                tracingBuffer?.Add(string.Format("Home station created on '{0}'.\n", planetToSettle.Name));

                                SquadViewChatHandlerBase chatHandlerOrNull = ChatClickHandler.CreateNewAs<SquadViewChatHandlerBase>( "ShipGeneralFocus" );
                                if ( chatHandlerOrNull != null )
                                    chatHandlerOrNull.SquadToView = LazyLoadSquadWrapper.Create( homestation );

                                var str = string.Format("<color=#{0}>{1}</color> have built their home station on {2}", AttachedFaction.FactionTrimColor.ColorHexBrighter, AttachedFaction.GetDisplayName(), planetToSettle.Name);

                                World_AIW2.Instance.QueueChatMessageOrCommand( str, ChatType.LogToCentralChat, chatHandlerOrNull );
                            }
                        }
                    }
                    debugCode = 120;
                }
                else
                {
                    UpdateHomeStation(hostCtx, tracingBuffer);
                }
                debugCode = 130;
                #endregion

                #region NearSpaceOutposts Update
                debugCode = 200;
                NearSpaceOutpostsLRP.Clear();
                AttachedFaction.DoForEntities(Reclaimers.NearOutpostType, delegate(GameEntity_Squad entity)
                {
                    NearSpaceOutpostsLRP.Add(entity);
                    return DelReturn.Continue;
                });

                if (tracingBuffer != null)
                    tracingBuffer.Add(string.Format("We have {0} {1} in LRP.\n", NearSpaceOutpostsLRP.Count, Reclaimers.NearOutpostType.DisplayName));
                debugCode = 210;
                
                UpdateNearSpaceOutposts(hostCtx, tracingBuffer);

                #endregion

                #region Planet Presence Update
                foreach (var pair in BaseInfo.PlanetDataLookup)
                    pair.Value.Presence = (FInt.FromParts(pair.Value.Presence,0) * Reclaimers.PlanetPresenceDecayPerSecond).GetNearestIntPreferringHigher();
                #endregion

                #region Constructors Update
                debugCode = 300;
                ConstructorsLRP.Clear();
                AttachedFaction.DoForEntities(Reclaimers.ConstructorType, delegate(GameEntity_Squad entity)
                {
                    ConstructorsLRP.Add(entity);
                    return DelReturn.Continue;
                });

                if (tracingBuffer != null)
                    tracingBuffer.Add(string.Format("We have {0} {1} in LRP.\n", ConstructorsLRP.Count, Reclaimers.ConstructorType.DisplayName));
                debugCode = 310;

                int numDesiredConstructors = 1 + (NearSpaceOutpostsLRP.Count / Reclaimers.NumNSOPerConstructor);
                if (ConstructorsLRP.Count < numDesiredConstructors)
                {
                    TryBuildConstructor(hostCtx, tracingBuffer);
                }
                else
                {
                    WanderConstructors(hostCtx, tracingBuffer);
                }
                #endregion

                #region DeepSpaceOutposts Update
                debugCode = 400;
                DeepSpaceOutpostsLRP.Clear();
                AttachedFaction.DoForEntities(Reclaimers.DeepOutpostType, delegate(GameEntity_Squad entity)
                {
                    DeepSpaceOutpostsLRP.Add(entity);
                    return DelReturn.Continue;
                });

                if (tracingBuffer != null)
                    tracingBuffer.Add(string.Format("We have {0} {1} in LRP.\n", DeepSpaceOutpostsLRP.Count, Reclaimers.DeepOutpostType.DisplayName));
                debugCode = 410;
                
                UpdateDeepOutposts(hostCtx, tracingBuffer);

                #endregion

                #region Research Ships Update
                debugCode = 500;
                ResearchShip.Clear();
                AttachedFaction.DoForEntities(Reclaimers.ResearchShipType, delegate(GameEntity_Squad entity)
                {
                    ResearchShip = SafeSquadWrapper.Create(entity);
                    return DelReturn.Break;
                });
                
                tracingBuffer?.Add(string.Format("We have {0} {1} in LRP.\n", ResearchShip.GetSquad() == null ? "0" : "1", Reclaimers.ResearchShipType.DisplayName));
                debugCode = 510;

                if (ResearchShip.GetSquad() == null)
                {
                    TryBuildResearchShip(hostCtx, tracingBuffer);
                }
                else
                {
                    UpdateResearchShip(hostCtx, tracingBuffer);
                }
                #endregion

                #region Warp Gates Update
                debugCode = 600;
                WarpGatesLRP.Clear();
                World_AIW2.Instance.DoForEntities(Reclaimers.WarpGateType, delegate(GameEntity_Squad entity)
                {
                    WarpGatesLRP.Add(SafeSquadWrapper.Create(entity));
                    return DelReturn.Continue;
                });

                if (tracingBuffer != null)
                    tracingBuffer.Add(string.Format("We have {0} {1} in LRP.\n", WarpGatesLRP.Count, Reclaimers.WarpGateType.DisplayName));
                debugCode = 610;

                UpdateWarpGates(hostCtx, tracingBuffer);
                #endregion

                #region Wormhole Suppressor Update
                debugCode = 620;
                WormholeSuppressorsLRP.Clear();

                World_AIW2.Instance.DoForEntities(Reclaimers.WormholeSuppressorType, delegate(GameEntity_Squad entity)
                {
                    WormholeSuppressorsLRP.Add(entity);
                        return DelReturn.Continue;
                });

                if (tracingBuffer != null)
                    tracingBuffer.Add(string.Format("We have {0} {1} in LRP.\n", WormholeSuppressorsLRP.Count, Reclaimers.WormholeSuppressorType.DisplayName));
                debugCode = 630;

                UpdateWormholeSuppressor(hostCtx, tracingBuffer);
                #endregion
#if false
                for ( int i = 0; i < ResearchShipsLRP.Count; i++)
                {
                    debugCode = 300;
                    GameEntity_Squad ship = ResearchShipsLRP[i].GetSquad();
                    if (ship == null)
                        continue;
                    ReclaimersPerUnitBaseInfo data =
                        ship.CreateExternalBaseInfo<ReclaimersPerUnitBaseInfo>("ReclaimersPerUnitBaseInfo");

                    
                    // movement

                    if (ship.GetDistanceTo_VeryCheapButExtremelyRough(data.ResearchShipDestinationPoint,
                        RadiusCheck.SubtractRadiiFromDistance) < 1000)
                    {
                        data.ResearchShipDestinationPoint = ArcenPoint.ZeroZeroPoint;
                    }

                    if (data.ResearchShipDestinationPoint == ArcenPoint.ZeroZeroPoint)
                    {
                        if (data.ResearchShipDestinationPlanet == ship.Planet ||
                            data.ResearchShipDestinationPlanet == null)
                        {
                            // pick destination planet
                            //var numLinks = ship.Planet.GetLinkedNeighborCount();
                            //if (numLinks > 0)
                            {
                                var n = ship.Planet.GetRandomNeighbor(false, Context);
                                data.ResearchShipDestinationPlanet = n;

                                data.ResearchShipDestinationPoint = n.GetRandomPointWithinCircleAndAlsoWithinGravWell(
                                    Engine_AIW2.Instance.CombatCenter, n.GravWellSize.DistanceScale_GravwellRadius / 2,
                                    Context.RandomToUse);
                            }
                        }
                    }

                    if (ship.HasQueuedOrders())
                        continue;
                    if (tracing)
                        tracingBuffer.Add(string.Format("Sending reclaimers research ship to: {0} on {1}\n", data.ResearchShipDestinationPoint, data.ResearchShipDestinationPlanet));
                    SendShipToLocation(ship, data.ResearchShipDestinationPlanet, data.ResearchShipDestinationPoint, Context, pathingCacheData);
                }

                // Research Ship Guards Movement

                debugCode = 1000;
                ResearchShipGuardsLRP.Clear();
                AttachedFaction.DoForEntities("ReclaimersResearchStationGuard", delegate(GameEntity_Squad entity)
                {
                    ResearchShipGuardsLRP.Add(entity);
                    return DelReturn.Continue;
                });

                if (tracing)
                    tracingBuffer.Add("We have " + ResearchShipGuardsLRP.Count + " reclaimers research ship guards in LRP.\n");
                debugCode = 1200;
                for (int i = 0; i < ResearchShipGuardsLRP.Count; i++)
                {
                    debugCode = 1300;

                    GameEntity_Squad ship = ResearchShipGuardsLRP[i].GetSquad();
                    if (ship == null)
                        continue;

                    ReclaimersPerUnitBaseInfo data =
                        ship.CreateExternalBaseInfo<ReclaimersPerUnitBaseInfo>("ReclaimersPerUnitBaseInfo");

                    GameEntity_Squad target = data.ResearchShipGuarded.GetSquad();
                    if (target == null)
                    {
                        int r = Context.RandomToUse.Next(ResearchShipsLRP.Count);
                        var g = ResearchShipsLRP[r];
                        
                        target = g.GetSquad();
                        data.ResearchShipGuarded = LazyLoadSquadWrapper.Create(target);
                    }

                    //if (ship.HasQueuedOrders())
                        //continue;

                    if ( ship.GetDistanceTo_VeryCheapButExtremelyRough( target.WorldLocation, RadiusCheck.SubtractRadiiFromDistance ) > 3000 )
                    {
                        GameCommand command = GameCommand.Create( BaseGameCommand.CommandsByCode[BaseGameCommand.Code.MoveManyToOnePoint_NPCFollowGuardedUnit], GameCommandSource.AnythingElse );
                        command.RelatedString = "ReclaimerResearchShipGuards_HugLeader";
                        command.RelatedEntityIDs.Add( ship.PrimaryKeyID );
                        command.RelatedPoints.Add( target.WorldLocation );
                        World_AIW2.Instance.QueueGameCommand( this.AttachedFaction, command, false );
                    }
                }

                // Outposts
                OutpostsLRP.Clear();
                AttachedFaction.DoForEntities("ReclaimersOutpost", delegate(GameEntity_Squad entity)
                {
                    OutpostsLRP.Add(entity);
                    return DelReturn.Continue;
                });
                if (tracing)
                    tracingBuffer.Add("We have " + OutpostsLRP.Count + " reclaimers outposts.\n");
#endif
                debugCode = 1300;
            }
            catch ( Exception e )
            {
                ArcenDebugging.ArcenDebugLogSingleLine( "Hit exception during reclaimers LRP debugCode " + debugCode + " " + e.ToString(), Verbosity.DoNotShow );
            }
            finally
            {
                TempPathCache.ReturnToPool();

                #region Tracing

                if ( tracing && !tracingBuffer.GetIsEmpty() )
                {
                    tracingBuffer.Add( this.TracingName ).Add( " " + AttachedFaction.FactionIndex ).Add( " LongRangePlanning trace ends at " ).Add( World_AIW2.Instance.GameSecond ).Add( " (+").Add(Elapsed).Add(")");
                    ArcenDebugging.ArcenDebugLogSingleLine( tracingBuffer.ToString(), Verbosity.DoNotShow );
                }
                if ( tracingBuffer != null )
                {
                    tracingBuffer.ReturnToPool();
                    tracingBuffer = null;
                }
                #endregion
            }
        }

        private void TryBuildResearchShip( ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer )
        {
            var homeStation = HomeStation.GetSquad();
            if (homeStation == null)
            {
                if (tracingBuffer != null)
                    tracingBuffer.Add("No home station to spawn research ship at...\n");
                return;
            }

            var homeStationPlanet = homeStation.Planet;

            var pfac = homeStationPlanet.GetPlanetFactionForFaction(AttachedFaction);

            var etype = Reclaimers.ResearchShipType;
            var loc = homeStationPlanet.GetSafePlacementPoint_AroundEntity(hostCtx, etype, homeStation, FInt.FromParts(0,10), FInt.FromParts(0,100));
            var researchShip = GameEntity_Squad.CreateNew_ReturnNullIfMPClient(pfac, etype, 1, null, 0, loc, hostCtx, "because");
            if (researchShip == null)
            {
                if (tracingBuffer != null)
                    tracingBuffer.Add("Failed to create a research ship, weird.\n");
            }
            else
            {
                researchShip.CreateExternalBaseInfo<ReclaimersResearchShipPerUnitBaseInfo>("ReclaimersResearchShipPerUnitBaseInfo");

                if (tracingBuffer != null)
                    tracingBuffer.Add(string.Format("Research ship created on '{0}'.\n", homeStationPlanet.Name));

                SquadViewChatHandlerBase chatHandlerOrNull = ChatClickHandler.CreateNewAs<SquadViewChatHandlerBase>( "ShipGeneralFocus" );
                if ( chatHandlerOrNull != null )
                    chatHandlerOrNull.SquadToView = LazyLoadSquadWrapper.Create( researchShip );

                var str = string.Format("<color=#{0}>{1}</color> have built a research ship on {2} to explore the galaxy.", AttachedFaction.FactionTrimColor.ColorHexBrighter, AttachedFaction.GetDisplayName(), homeStationPlanet.Name);

                World_AIW2.Instance.QueueChatMessageOrCommand( str, ChatType.LogToCentralChat, chatHandlerOrNull );
            }
        }

        private void UpdateResearchShip( ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer )
        {
            var squad = ResearchShip.GetSquad();
            if (squad == null)
                return;
            var data = squad.ResearchShipData();
            if (data == null)
                return;

            #region Wander Update
            var curPlanet = squad.Planet;
            foreach ( KeyValuePair<GameEntity_Squad, int> kv in World_AIW2.Instance.CurrentHackers )
            {
                var hacker = kv.Key;
                GameEntity_Squad target = World_AIW2.Instance.GetEntityByID_Squad( hacker.ActiveHack_Target );
                if (target != null && target == squad)
                {
                    if (tracingBuffer != null)
                        tracingBuffer.Add(string.Format("Early out of UpdateResearchShip because being hacked.\n"));

                    if (squad.Orders.GetQueuedOrderCount() > 0)
                    {
                        squad.Orders.ClearOrders(ClearBehavior.DoNotClearBehaviors, ClearDecollisionOnParent.DoNotClearDecollision, ClearSource.YesClearAnyOrders_IncludingFromHumans, "Being hacked, hold still");

                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("   Clearing orders.\n"));
                    }

                    return;
                }
            }
            
            // don't even have a dstplanet? pick one
            if (data.DstPlanet == null)
            {
                if (tracingBuffer != null)
                    tracingBuffer.Add( string.Format( "Picking new DstPlanet for wandering research ship.\n" ) );

                bool ratePlanet(Planet p, out PlanetRating rating)
                {
                    rating = new PlanetRating();
                    if ( p.HasPlanetBeenDestroyed )
                        return false;
                    if ( !p.GetIsDirectlyLinkedTo( false, curPlanet ) )
                        return false;
                    if ( p == curPlanet )
                        return false;

                    // Prefer going were we did not just come from.
                    if ( p != data.LastPlanet )
                        rating.Tier = 1;
                    
                    // Prefer planets that are explored, slightly
                    if ( p.GetDoHumansHaveExplored() )
                        rating.Rating = 100;
                    else
                        rating.Rating = 80;

                    // Prefer planets that have science (so, not claimed by the player, now or previously)
                    if ( p.GetScienceLeftForHumans() <= 0)
                        rating.Rating /= 2;

                    return true;
                }

                data.DstPlanet = World_AIW2.Instance.CurrentGalaxy.GetRandomPlanet(hostCtx, ratePlanet, tracingBuffer );
                data.DstPos = ArcenPoint.ZeroZeroPoint;
                data.LastPlanet = curPlanet;

                if (data.DstPlanet == null)
                {
                    // then just wander this planet again
                    data.DstPlanet = curPlanet;
                    data.WanderCounter = 0;
                    data.DstPos = ArcenPoint.ZeroZeroPoint;

                    if (tracingBuffer != null)
                        tracingBuffer.Add( string.Format( "No new planet valid to wander to, re-wandering this planet.\n" ) );
                }
                else
                { 
                    if (tracingBuffer != null)
                        tracingBuffer.Add(string.Format("Picked a new DstPlanet '{0}'.\n", data.DstPlanet.Name));
                }
            }
            // not at right planet? go there
            if (squad.Planet != data.DstPlanet)
            {
                if (squad.Orders.GetFinalDestinationOrNull() != data.DstPlanet)
                {
                    data.DstPos = ArcenPoint.ZeroZeroPoint;
                    data.WanderCounter = 0;
                    SendShipToPlanet(squad, data.DstPlanet, hostCtx, TempPathCache);

                    if (tracingBuffer != null)
                        tracingBuffer.Add(string.Format("Moving to a new DstPlanet '{0}'.\n", data.DstPlanet.Name));
                }
            }
            // rest of update for ships already at DstPlanet
            else 
            {
                // don't have a dstpos? pick one
                if (data.DstPos == ArcenPoint.ZeroZeroPoint)
                {
                    // wander several times if we haven't yet
                    if (data.WanderCounter < Reclaimers.TimesToWander_Constructor)
                    {
                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("Picking a new DstPos for wandering.\n"));

                        data.DstPos = curPlanet.GetSafePlacementPointAroundPlanetCenter(hostCtx, squad.TypeData, FInt.FromParts(0, 300), FInt.FromParts(0, 800));
                        data.WanderCounter++;
                    }
                    // we have wandered enough, change planets next time
                    else
                    {
                        data.DstPlanet = null;

                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("Nothing to do here, lets go to a new planet.\n"));
                    }
                }
                // have we reached our destination?
                else if (squad.WorldLocation.GetDistanceTo(data.DstPos, true) < 100)
                {
                    if ( data.WanderCounter < Reclaimers.TimesToWander_Constructor )
                    { 
                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("Reached wander position and still wandering.\n"));
                    }
                    else
                    {
                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("Reached DstPos and nothing to do.\n"));
                    }

                    data.DstPos = ArcenPoint.ZeroZeroPoint;
                }
                // are we moving towards our destination?
                else if (squad.Orders.GetQueuedOrderCount() == 0)
                {
                    SendShipToLocation(squad, data.DstPlanet, data.DstPos, hostCtx, TempPathCache);

                    if (tracingBuffer != null)
                        tracingBuffer.Add(string.Format("Moving to DstPos '{0}'.\n", data.DstPos));
                }
            }
            #endregion
        }

        private void UpdateWarpGates( ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer )
        {
            foreach (var a in WarpGatesLRP)
            {
                var squad = a.GetSquad();
                if (squad == null)
                    continue;

                var dataa = squad.WarpGateData();
                if (dataa == null)
                    continue;

                if (squad.GetHasBeenDestroyed() || squad.SelfBuildingMetalRemaining > 0)
                    continue;
                
                GameEntity_Squad other = null;

                foreach ( var b in WarpGatesLRP )
                { 
                    var squadb = b.GetSquad();
                    if (squadb == null)
                        continue;

                    var datab = squadb.WarpGateData();
                    if (datab == null)
                        continue;

                    if (squad == squadb)
                        continue;

                    if (squadb.GetHasBeenDestroyed() || squadb.SelfBuildingMetalRemaining > 0)
                        continue;

                    other = squadb;
                }

                if ( dataa.WormholeToPlanet == null )
                {
                    if ( other != null )
                    {
                        // we dont have a wormhole, but we have a pair, make a wormhole

                        var planeta = squad.Planet;
                        var planetb = other.Planet;

                        if ( planeta.GetIsDirectlyLinkedTo( false, planetb ) )
                        {
                            // it was already created by the other guy
                            if ( other.WarpGateData().WormholeToPlanet == planeta )
                            {
                                dataa.WormholeToPlanet = planetb;
                                ArcenDebugging.ArcenDebugLogSingleLine( string.Format( "[reclaimers] marking WarpGate as Created because other already did it" ), Verbosity.DoNotShow );
                            }

                            // looks like planets are naturally linked, so nothing to do
                        }
                        else if (planeta != planetb)
                        {
                            // time to create a wormhole

                            ArcenPoint posa, posb;
                            posa = planeta.GetSafePlacementPoint_AroundEntity( hostCtx, Reclaimers.WarpGateType, squad, FInt.FromParts( 0, 015 ), FInt.FromParts( 0, 030 ) );
                            posb = planetb.GetSafePlacementPoint_AroundEntity( hostCtx, Reclaimers.WarpGateType, other, FInt.FromParts( 0, 015 ), FInt.FromParts( 0, 030 ) );

                            var cmd = GameCommand.Create( BaseGameCommand.CommandsByCode[BaseGameCommand.Code.LinkPlanets], GameCommandSource.AnythingElse );
                            cmd.RelatedIntegers.Add( planeta.Index );
                            cmd.RelatedIntegers.Add( planetb.Index );
                            cmd.RelatedPoints.Add( posa );
                            cmd.RelatedPoints.Add( posb );
                            World_AIW2.Instance.QueueGameCommand( this.AttachedFaction, cmd, false );

                            ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format( "[claim] linking '{0}' and '{1}'", planeta.Name, planetb.Name ),
                                Verbosity.DoNotShow );

                            World_AIW2.Instance.CurrentGalaxy.RecomputeDestinationIndexToWormholeMapping();
                            World_AIW2.Instance.CurrentGalaxy.RecomputePlanetDistances();
                            World_AIW2.Instance.CurrentGalaxy.RecomputeLinkedPathfindables();

                            dataa.WormholeToPlanet = planetb;
                            other.WarpGateData().WormholeToPlanet = planeta;
                        }
                    }
                }
                else
                {
                    if (other == null)
                    {
                        // we have a wormhole, but no pair, destroy a wormhole

                        var planeta = squad.Planet;
                        var planetb = dataa.WormholeToPlanet;

                        var cmd = GameCommand.Create( BaseGameCommand.CommandsByCode[BaseGameCommand.Code.UnlinkPlanets], GameCommandSource.AnythingElse );
                        cmd.RelatedIntegers.Add( planeta.Index );
                        cmd.RelatedIntegers.Add( planetb.Index );
                        World_AIW2.Instance.QueueGameCommand( this.AttachedFaction, cmd, false );

                        ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format( "[claim] unlinking '{0}' and '{1}'", planeta.Name, planetb.Name ),
                                Verbosity.DoNotShow );

                        World_AIW2.Instance.CurrentGalaxy.RecomputeDestinationIndexToWormholeMapping();
                        World_AIW2.Instance.CurrentGalaxy.RecomputePlanetDistances();
                        World_AIW2.Instance.CurrentGalaxy.RecomputeLinkedPathfindables();

                        dataa.WormholeToPlanet = null;
                    }
                }
            }
        }

        private void UpdateWormholeSuppressor( ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer )
        {
            foreach (var a in WormholeSuppressorsLRP)
            {
                var squad = a.GetSquad();
                if (squad == null)
                    continue;

                var dataa = squad.WormholeSuppressorData();
                if (dataa == null)
                    continue;

                if (squad.GetHasBeenDestroyed() || squad.SelfBuildingMetalRemaining > 0)
                    continue;
                
                if (dataa.Triggered)
                    continue;

                int dist = int.MaxValue;
                GameEntity_Other other = null;
                Planet otherPlanet = null;
                squad.Planet.DoForEntities(OtherSpecialEntityType.Wormhole,
                    (GameEntity_Other e)=>
                    {
                        var d = Mat.DistanceBetweenPointsSIMD(squad.WorldLocation, e.WorldLocation);
                        if (d < dist)
                        {
                            var tmp = World_AIW2.Instance.GetPlanetByIndex(e.LinkedPlanetIndex);
                            if (tmp != null)
                            {
                                dist = d;
                                other = e;
                                otherPlanet = tmp;
                            }
                        }
                        return DelReturn.Continue;
                    } );

                if (other == null)
                    continue;

                dataa.Triggered = true;
                dataa.Wormhole = other.PrimaryKeyID;

                GameCommand createCommand = GameCommand.Create( BaseGameCommand.CommandsByCode[BaseGameCommand.Code.UnlinkPlanets], GameCommandSource.AnythingElse );
                createCommand.RelatedIntegers.Add(squad.Planet.Index);
                createCommand.RelatedIntegers.Add(otherPlanet.Index);
                World_AIW2.Instance.QueueGameCommand( this.AttachedFaction, createCommand, false );

                ArcenDebugging.ArcenDebugLogSingleLine( string.Format( "[claim] wormhole from {0} to {1} is now suppressed", 
                    squad.Planet.Name, otherPlanet.Name), Verbosity.DoNotShow );
            }
        }

        private void UpdateNearSpaceOutposts( ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer )
        {
            NearSpaceOutpostsLRP.ForEach((wrapper)=>
            {
                var squad = wrapper.GetSquad();
                if (squad == null)
                    return;
                var nsdata = squad.NsoData();
                if (nsdata == null)
                    return;
                var mem = squad.FleetMembership;
                var fleet = squad.GetFleetOrNull_Safe();

                if (BaseInfo.DebugMode)
                {
                    var metal_amt = Reclaimers.PassiveMetalIncome * Elapsed * Reclaimers.MetalReclamationRate / 100;
                    var exp_amt = Reclaimers.PassiveMetalIncome * Elapsed * Reclaimers.ExpRate / 100;

                    nsdata.Metal += metal_amt;
                    nsdata.MetalTotal += metal_amt;

                    if (squad.CurrentMarkLevel >= 7)
                        nsdata.Exp = 0;
                    else
                        nsdata.Exp += exp_amt;
                }

                if (nsdata.Metal >= Reclaimers.DefenseFighterMetalCost )
                {
                    var playerFaction = World_AIW2.Instance.GetLocalPlayerFactionOrNull();
                    if (playerFaction != null)
                    {
                        var mem2 = fleet.GetOrAddMembershipGroupBasedOnSquadType_AssumeNoDuplicates(Reclaimers.PlanetaryDefenderTypeA);
                        mem2.ExplicitBaseSquadCap += hostCtx.RandomToUse.NextInclus(5, 10);
                        var mem3 = fleet.GetOrAddMembershipGroupBasedOnSquadType_AssumeNoDuplicates(Reclaimers.PlanetaryDefenderTypeB);
                        mem3.ExplicitBaseSquadCap += hostCtx.RandomToUse.NextInclus(5, 10);

                        /*
                        SquadViewChatHandlerBase chatHandlerOrNull = ChatClickHandler.CreateNewAs<SquadViewChatHandlerBase>( "ShipGeneralFocus" );
                        if ( chatHandlerOrNull != null )
                            chatHandlerOrNull.SquadToView = LazyLoadSquadWrapper.Create( squad );
                        var str = string.Format("<color=#{0}>{1}</color> have refurbished strikecraft for defending {2}", AttachedFaction.FactionTrimColor.ColorHexBrighter, AttachedFaction.GetDisplayName(), squad.Planet.Name);
                        World_AIW2.Instance.QueueChatMessageOrCommand( str, ChatType.LogToCentralChat, chatHandlerOrNull );
                        */

                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("[claim] reward strikecraft spawned at outpost on {0}", squad.Planet.Name));

                        nsdata.Metal -= Reclaimers.DefenseFighterMetalCost;
                    }
                }

                if (nsdata.Exp >= Reclaimers.MarkupExpCost)
                {
                    if (squad.CurrentMarkLevel < 7)
                    {
                        byte mark = (byte)(squad.CurrentMarkLevel + 1);
                        squad.SetCurrentMarkLevel(mark);
                        nsdata.Exp -= Reclaimers.MarkupExpCost;

                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("[claim] Upgrading mark level of '{0}' to '{1}'\n", squad.GetTypeDisplayNameSafe(), squad.CurrentMarkLevel));
                    }
                    else
                    {
                        // to turn of showing % progress towards next mark when at max
                        nsdata.Exp = 0;
                    }
                }
            });
        }

        private void TryBuildConstructor(ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer)
        {
            var homeSquad = HomeStation.GetSquad();
            if (homeSquad == null)
                return;

            var homeSquadPlanet = homeSquad.Planet;
            var constructorType = GameEntityTypeDataTable.Instance.GetRowByName("ReclaimersConstructor");
            var constructorLoc = homeSquadPlanet.GetSafePlacementPoint_AroundEntity(hostCtx, constructorType, homeSquad, FInt.FromParts(0,10), FInt.FromParts(0,100));

            var constructor = GameEntity_Squad.CreateNew_ReturnNullIfMPClient(homeSquadPlanet.GetPlanetFactionForFaction(AttachedFaction), constructorType, 1, null, 0, constructorLoc, hostCtx, "because");
            if (constructor == null)
            {
                if (tracingBuffer != null)
                    tracingBuffer.Add("[claim] Failed to create constructor, weird.\n");
            }
            else
            {
                if (tracingBuffer != null)
                    tracingBuffer.Add(string.Format("[claim] Constructor created on '{0}'.\n", homeSquadPlanet.Name));

                var info = constructor.CreateExternalBaseInfo<ReclaimersConstructorPerUnitBaseInfo>("ReclaimersConstructorPerUnitBaseInfo");
                info.DstPlanet = homeSquadPlanet;
            }
        }

        private void WanderConstructors(ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer)
        {
            ConstructorsLRP.ForEach((wrapper)=>
            {
                var squad = wrapper.GetSquad();
                if (squad == null)
                    return;
                var data = squad.ConstructorData();
                if (data == null)
                    return;

                data.Money += Elapsed * Reclaimers.ConstructorMoneyPerSecond * (BaseInfo.DebugMode ? 10 : 1);
                if (data.Money > Reclaimers.ConstructorMaxMoney)
                    data.Money = Reclaimers.ConstructorMaxMoney;

                var curPlanet = squad.Planet;
                var curPlanetOutpost = curPlanet.GetNearOutpost();

                PlanetData planetData;
                if (!BaseInfo.PlanetDataLookup.TryGetValue(curPlanet.Index, out planetData))
                {
                    planetData = new PlanetData();
                    BaseInfo.PlanetDataLookup.Add(curPlanet.Index, planetData);
                }
                planetData.Presence += Elapsed * Reclaimers.PlanetPresenceGainPerSecond;
                if (planetData.Presence > Reclaimers.PlanetPresenseMax)
                    planetData.Presence = Reclaimers.PlanetPresenseMax;

                /*
                if (curPlanetOutpost != null)
                {
                    if (tracingBuffer != null)
                        tracingBuffer.Add( string.Format( "'{0}' is '{1}'\n", curPlanetOutpost.TypeData.DisplayName, curPlanetOutpost.IsMobileOrCountsAsMobileDueToOrbitingSomethingMobile() ? "mobile" : "not-mobile"));
                }
                */
                // don't even have a dstplanet? pick one
                if (data.DstPlanet == null)
                {
                    if (tracingBuffer != null)
                        tracingBuffer.Add(string.Format("No dstplanet lets pick one.\n"));

                    bool ratePlanet(Planet p, out PlanetRating rating)
                    {
                        rating = new PlanetRating();
                        if ( p.HasPlanetBeenDestroyed )
                            return false;
                        if ( !p.GetIsDirectlyLinkedTo( false, curPlanet ) )
                            return false;
                        if ( p == curPlanet )
                            return false;
                        if ( p.GetStrengthOfFactions_HostileTo( AttachedFaction ) > 10000 )
                        {
                            if (tracingBuffer != null)
                                tracingBuffer.Add( string.Format( "'{0}' has '{1}' enemy strength\n", p.Name, p.GetStrengthOfFactions_HostileTo( AttachedFaction )));
                            return false;
                        }
                        
                        rating.Tier = 1;

                        // always perfer planets that don't already have an outpost
                        bool hasOutpost = false;
                        p.DoForEntities(Reclaimers.NearOutpostType, (GameEntity_Squad e) =>
                        {
                            hasOutpost = true;
                            return DelReturn.Break;
                        } );

                        if (hasOutpost)
                            rating.Tier = 0;

                        if ( p == data.LastPlanet)
                            rating.Tier = 0;

                        // weight is the presence inverted, so given two equal choices, goto the planet we've been to the least
                        rating.Rating = (uint)Reclaimers.PlanetPresenseMax;
                        PlanetData pd;
                        if (BaseInfo.PlanetDataLookup.TryGetValue(p.Index, out pd))
                            rating.Rating -= (uint)pd.Presence;

                        return true;
                    }

                    data.DstPlanet = World_AIW2.Instance.CurrentGalaxy.GetRandomPlanet(hostCtx, ratePlanet, tracingBuffer );
                    data.DstPos = ArcenPoint.ZeroZeroPoint;

                    if (data.DstPlanet == null)
                    {
                        // then just wander this planet again
                        data.DstPlanet = curPlanet;
                        data.WanderCounter = 0;
                        data.DstPos = ArcenPoint.ZeroZeroPoint;

                        if (tracingBuffer != null)
                            tracingBuffer.Add( string.Format( "No new planet valid to wander to, re-wandering this planet.\n" ) );
                    }
                    else
                    { 
                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("Picked a new DstPlanet '{0}'.\n", data.DstPlanet.Name));
                    }
                }
                // not at right planet? go there
                if (squad.Planet != data.DstPlanet)
                {
                    if (squad.Orders.GetFinalDestinationOrNull() != data.DstPlanet)
                    {
                        data.DstPos = ArcenPoint.ZeroZeroPoint;
                        data.WanderCounter = 0;
                        SendShipToPlanet(squad, data.DstPlanet, hostCtx, TempPathCache);

                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("Moving to a new DstPlanet '{0}'.\n", data.DstPlanet.Name));
                    }
                }
                // rest of this update is only for ships actually at their DstPlanet
                else
                {
                    // don't have a dstpos? pick one
                    if (data.DstPos == ArcenPoint.ZeroZeroPoint)
                    {
                        // wander several times if we haven't yet
                        if (data.WanderCounter < Reclaimers.TimesToWander_Constructor)
                        {
                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("Picking a new DstPos for wandering.\n"));

                            data.DstPos = curPlanet.GetSafePlacementPointAroundPlanetCenter(hostCtx, squad.TypeData, FInt.FromParts(0, 300), FInt.FromParts(0, 800));
                            data.WanderCounter++;
                        }
                        // planet no outpost, can we build one?
                        // edit: the constructor can actually build at the dstpos of any of its wander moves too so this will actually not ever hit...
                        else if (curPlanetOutpost == null && data.Money >= 100)
                        {
                            var done = false;
                            int tries = 20;
                            var min = FInt.FromParts( 0, 300 );
                            var max = FInt.FromParts( 0, 800 );
                            while (!done)
                            {
                                var pos = data.DstPos = curPlanet.GetSafePlacementPointAroundPlanetCenter(hostCtx, squad.TypeData, min, max);
                                var dist = Math.Sqrt( pos.X*pos.X + pos.Y*pos.Y ) / (float)curPlanet.GravWellSize.DistanceScale_GravwellRadius;

                                bool collides = false;
                                curPlanet.DoForWormholes(
                                    ( GameEntity_Other e ) =>
                                    {
                                        var vec = e.WorldLocation - Engine_AIW2.Instance.CombatCenter;
                                        var len = UnityEngine.Mathf.Sqrt( vec.X * vec.X + vec.Y * vec.Y );
                                        var norm = (float)len / (float)curPlanet.GravWellSize.DistanceScale_GravwellRadius;
                                        var dif = Math.Abs( dist - norm );
                                        //tracingBuffer?.Add(string.Format("pos={0} dist={1} vec={2} len={3} norm={4} dif={5} dif*100={6}.\n", pos, dist, vec, len, norm, dif, dif*100));
                                        if ( (dif * 100) < 25 )
                                        {
                                            tracingBuffer?.Add(string.Format("Rejected a pos {0} for outpost because too close to a wormhole radius.\n", pos));
                                            collides = true;
                                            return DelReturn.Break;
                                        }
                                        return DelReturn.Continue;
                                    });

                                if ( collides && tries > 0)
                                {
                                    tries--;
                                    continue;
                                }

                                break;
                            }

                            tracingBuffer?.Add(string.Format("Picked a new DstPos for outpost building.\n"));
                        }

                        // planet has an outpost, can we upgrade it?
                        /*
                        else if (curPlanetOutpost != null && curPlanetOutpost.CurrentMarkLevel < 7 && curPlanetOutpost.NsoData().Exp >= Reclaimers.MarkupExpCost)
                        {
                            data.DstPos = curPlanet.GetSafePlacementPoint_AroundEntity(hostCtx, squad.TypeData, curPlanetOutpost, FInt.FromParts(0, 025), FInt.FromParts(0, 075));

                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("Picking a new DstPos for outpost upgrading.\n"));
                        }
                        */
                        // we have wandered enough, change planets next time
                        else
                        {
                            data.DstPlanet = null;

                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("Nothing to do here, lets go to a new planet.\n"));
                        }
                    }
                    // have we reached our destination?
                    else if (squad.WorldLocation.GetDistanceTo(data.DstPos, true) < 100)
                    {
                        if ( data.WanderCounter < Reclaimers.TimesToWander_Constructor )
                        { 
                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("Reached wander position and still wandering.\n"));
                        }
                        else if (curPlanetOutpost == null && data.Money >= 100)
                        {
                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("Building a new outpost.\n", data.DstPos));

                            ArcenPoint newOutpostLoc = curPlanet.GetSafePlacementPoint_AroundDesiredPointVicinity(hostCtx, Reclaimers.NearOutpostType, data.DstPos, 0, 100);
                            var newOutpost = GameEntity_Squad.CreateNew_ReturnNullIfMPClient(curPlanet.GetPlanetFactionForFaction(AttachedFaction), Reclaimers.NearOutpostType, 1, null, 0, newOutpostLoc, hostCtx, "because");
                            if (newOutpost == null)
                            {
                                if (tracingBuffer != null)
                                    tracingBuffer.Add(string.Format("Failed creating a new '{0}'\n", Reclaimers.NearOutpostType.DisplayName));
                            }
                            else
                            {
                                SquadViewChatHandlerBase chatHandlerOrNull = ChatClickHandler.CreateNewAs<SquadViewChatHandlerBase>( "ShipGeneralFocus" );
                                if ( chatHandlerOrNull != null )
                                    chatHandlerOrNull.SquadToView = LazyLoadSquadWrapper.Create( newOutpost );
                                var str = string.Format("<color=#{0}>{1}</color> have built a new {2} on {3}", AttachedFaction.FactionTrimColor.ColorHexBrighter, AttachedFaction.GetDisplayName(), newOutpost.GetTypeDisplayNameSafe(), newOutpost.Planet.Name);
                                World_AIW2.Instance.QueueChatMessageOrCommand( str, ChatType.LogToCentralChat, chatHandlerOrNull );
                            }
                            data.Money -= 100;
                        }
                        /*
                        else if (curPlanetOutpost != null && curPlanetOutpost.CurrentMarkLevel < 7 && curPlanetOutpost.NsoData().Exp >= Reclaimers.MarkupExpCost)
                        {
                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("Upgrading mark level of '{0}' to '{1}'\n", curPlanetOutpost.GetTypeDisplayNameSafe(), curPlanetOutpost.CurrentMarkLevel + 1));

                            byte mark = (byte)(curPlanetOutpost.CurrentMarkLevel + 1);
                            curPlanetOutpost.SetCurrentMarkLevel(mark);
                            curPlanetOutpost.NsoData().Exp -= Reclaimers.MarkupExpCost;
                        }
                        */
                        else
                        {
                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("Reached DstPos and nothing to do.\n"));
                        }

                        data.DstPos = ArcenPoint.ZeroZeroPoint;
                    }
                    // are we moving towards our destination?
                    else if (squad.Orders.GetQueuedOrderCount() == 0)
                    {
                        SendShipToLocation(squad, data.DstPlanet, data.DstPos, hostCtx, TempPathCache);

                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("Moving to DstPos '{0}'.\n", data.DstPos));
                    }
                }
            } );
        }

        private void UpdateHomeStation(ArcenHostOnlySimContext hostCtx, ArcenCharacterBuffer tracingBuffer)
        {
            int debugCode = 0;
            try
            {
                var squad = HomeStation.GetSquad();
                if (squad == null)
                    return;
                var hdata = squad.HomeData();
                if (hdata == null)
                    return;

                debugCode = 10;

                if (BaseInfo.DebugMode)
                {
                    var metal_amt = Reclaimers.PassiveMetalIncome * Elapsed * Reclaimers.MetalReclamationRate / 100;
                    //var res_amt = Reclaimers.PassiveMetalIncome * Elapsed * Reclaimers.ResearchRate / 100;

                    hdata.Metal += metal_amt;
                    hdata.MetalSinceStart += metal_amt;
                    //hdata.Research += res_amt;
                }

                #region Update Research
                // research gained is like a timer counting up to the research unlock
                //   this only happens when below the max unlocks per aip mark level
                {
                    int highestAiMarkLevel = 0;
                    for ( int i = 0; i < World_AIW2.Instance.AIFactions.Count; i++)
                    {
                        Faction faction = World_AIW2.Instance.AIFactions[i];
                        int currentMarkLevelForAI = faction.CurrentGeneralMarkLevel;
                        if (currentMarkLevelForAI > highestAiMarkLevel)
                            highestAiMarkLevel = currentMarkLevelForAI;
                    }

                    int maxUnlocks = highestAiMarkLevel * Reclaimers.ResearchUnlocksPerAipMark;
                    int currentUnlocks = 0;
                    foreach (var pair in BaseInfo.RewardCapLookup.GetDisplayDict())
                    {
                        if (pair.Value > 0)
                            currentUnlocks++;
                    }

                    if (currentUnlocks < maxUnlocks)
                    {
                        var res_amt = Elapsed * Reclaimers.ResearchRate;
                        hdata.Research += res_amt;
                        hdata.ResearchBlocked = false;
                    }
                    else
                    {
                        hdata.ResearchBlocked = true;
                    }
                }
                #endregion

                debugCode = 20;

                if (hdata.NextReward == null)
                {
                    GameEntityTypeData nextReward = null;
                    {
                        var rewards = GameEntityTypeDataTable.Instance.GetAllRowsWithTagOrNull("Reclaimers_Reward");
                        EntityTypeDrawBag.Clear();
                        foreach (var r in rewards)
                        {
                            if ( r.IsHidden )
                                continue;
                            if (BaseInfo.RewardCapLookup.Display[r.InternalName] == 0)
                                EntityTypeDrawBag.AddItem(r, 1);
                        }
                        if (EntityTypeDrawBag.HasAnyContent)
                            nextReward = EntityTypeDrawBag.PickRandomItemAndDoNotReplace(hostCtx.RandomToUse);
                    }

                    hdata.NextReward = nextReward;
                }

                // NextRewardCost is not serialized, we set it each time here
                if (hdata.NextReward != null)
                    hdata.NextRewardCost = hdata.NextReward.GetCustomInt32_Slow("custom_int_Reclaimers_RewardCost");
                else
                    hdata.NextRewardCost = 0;

                debugCode = 100;

                if (hdata.NextReward != null && hdata.NextRewardCost <= hdata.Research)
                {
                    var playerFaction = World_AIW2.Instance.GetLocalPlayerFactionOrNull();
                    if (playerFaction != null)
                    {
                        var iname = hdata.NextReward.InternalName;
                        
                        // this little dance is so we set both the construction and display lists
                        // is it safe? who knows!
                        BaseInfo.RewardCapLookup.GetDisplayDict()[iname] = 1;
                        BaseInfo.RewardCapLookup.SwitchConstructionToDisplay();
                        BaseInfo.RewardCapLookup.SetToConstructionDict(iname, 1);
                        BaseInfo.RewardCapLookup.SwitchConstructionToDisplay();

                        SquadViewChatHandlerBase chatHandlerOrNull = ChatClickHandler.CreateNewAs<SquadViewChatHandlerBase>( "ShipGeneralFocus" );
                        if ( chatHandlerOrNull != null )
                        {
                            var ship = ResearchShip.GetSquad();
                            if ( ship != null )
                                chatHandlerOrNull.SquadToView = LazyLoadSquadWrapper.Create( ship );
                        }

                        var col = AttachedFaction.FactionTrimColor.ColorHexBrighter;
                        var str = string.Format("<color=#{0}>{1}</color> have developed new infrastructure: {2}. Find and hack the wandering <color=#{0}>Research Ship</color> to obtain it.", col, AttachedFaction.GetDisplayName(), hdata.NextReward.GetShortDisplayName());
                        World_AIW2.Instance.QueueChatMessageOrCommand( str, ChatType.LogToCentralChat, "AnyEnemiesAlarm", chatHandlerOrNull, 20.0f );

                        if (tracingBuffer != null)
                            tracingBuffer.Add(string.Format("reward infrastructure {0} cap is now {1}", hdata.NextReward.GetShortDisplayName(), BaseInfo.RewardCapLookup.Display[iname]));
                 
                        hdata.Research -= hdata.NextRewardCost;
                        hdata.NextReward = null;
                        hdata.NextRewardCost = 0;
                    }
                }

                debugCode = 200;

                if (hdata.Metal >= Reclaimers.ScrapBoxMetalCost)
                {
                    int numBoxes = 0;
                    squad.Planet.DoForEntities( Reclaimers.ScrapBoxType, 
                        (GameEntity_Squad e)=>
                        {
                            numBoxes++;
                            return DelReturn.Continue;
                        } );

                    if (BaseInfo.MaxScrapBoxes > numBoxes)
                    {
                        var playerFaction = World_AIW2.Instance.GetLocalPlayerFactionOrNull();
                        if (playerFaction != null)
                        {
                            debugCode = 210;

                            var rewardLoc = squad.Planet.GetSafePlacementPoint_AroundEntity(hostCtx, Reclaimers.ScrapBoxType, squad, FInt.FromParts(0,10), FInt.FromParts(0,100));
                            var rewardEntity = GameEntity_Squad.CreateNew_ReturnNullIfMPClient(squad.Planet.GetPlanetFactionForFaction(playerFaction), Reclaimers.ScrapBoxType, 1, null, 0, rewardLoc, hostCtx, "because");
                        
                            debugCode = 220;

                            /*
                            SquadViewChatHandlerBase chatHandlerOrNull = ChatClickHandler.CreateNewAs<SquadViewChatHandlerBase>( "ShipGeneralFocus" );
                            if ( chatHandlerOrNull != null )
                                chatHandlerOrNull.SquadToView = LazyLoadSquadWrapper.Create( rewardEntity );
                            debugCode = 221;
                            var str = string.Format("<color=#{0}>{1}</color> have consolidated a Scrapbox on {2}", AttachedFaction.FactionTrimColor.ColorHexBrighter, AttachedFaction.GetDisplayName(), rewardEntity.Planet.Name);
                            debugCode = 222;
                            World_AIW2.Instance.QueueChatMessageOrCommand( str, ChatType.LogToCentralChat, chatHandlerOrNull );
                            */

                            debugCode = 230;

                            if (tracingBuffer != null)
                                tracingBuffer.Add(string.Format("reward {0} spawned at home station on {1}", rewardEntity.GetFactionDisplayNameSafe(), squad.Planet.Name));

                            debugCode = 240;
                        }

                        hdata.Metal -= Reclaimers.ScrapBoxMetalCost;
                    }
                }

                debugCode = 300;
            }
            catch (Exception e)
            {
                ArcenDebugging.ArcenDebugLogSingleLine( string.Format("debugCode '{0}\n{1}", debugCode, e), Verbosity.DoNotShow );
            }
        }

        private void UpdateDeepOutposts(ArcenHostOnlySimContext context, ArcenCharacterBuffer tracing)
        {
            if (BaseInfo.TillNextOutpost == -1)
            {
                // initial value, wait this long before the first outpost
                BaseInfo.TillNextOutpost = Reclaimers.TillFirstOutpost;
            }
            else
            {
                BaseInfo.TillNextOutpost -= Elapsed;
                if (BaseInfo.TillNextOutpost <= 0)
                {
                    BaseInfo.TillNextOutpost = BaseInfo.OutpostShuffleTime;

                    if (DeepSpaceOutpostsLRP.Count < BaseInfo.MaxDeepSpaceOutposts)
                    {
                        BaseInfo.LastOutpostFailed = false;
                        CreateDeepOutposts(context, tracing);

                        if (BaseInfo.LastOutpostFailed)
                        {
                            if ( tracing != null )
                                tracing.Add( string.Format( "Since we failed creating an outpost, remove one.\n" ) );

                            RemoveDeepOutpost(context, tracing);
                            BaseInfo.LastOutpostFailed = false;
                        }
                    }
                    else if (DeepSpaceOutpostsLRP.Count > 0)
                    {
                        RemoveDeepOutpost(context, tracing);
                    }
                }
            }
        }

        private void CreateDeepOutposts(ArcenHostOnlySimContext context, ArcenCharacterBuffer tracing)
        { 
            int debugStage = 0;
            var galaxy = World_AIW2.Instance.CurrentGalaxy;
            List<Planet> candidatePlanets = Planet.GetTemporaryPlanetList("ExodianBladeFactionDeepInfo-UpdateDeepOutposts-candidatePlanets", 20f );
            try
            {
                if ( tracing != null )
                    tracing.Add( string.Format( "Picking Deep Space Outpost locations...\n" ) );

                debugStage = 100;

                int retries = 0;
                while ( retries < 5 )
                {
                    if ( tracing != null )
                        tracing.Add( string.Format( "Try #{0}...\n", retries + 1 ) );

                    candidatePlanets.Clear();
                    foreach ( var i in DeepSpaceOutpostsLRP )
                    {
                        var squad = i.GetSquad();
                        if ( squad == null )
                            continue;
                        candidatePlanets.Add( squad.Planet );
                    }

                    debugStage = 200;

                    //for ( int i = 0; i < 1; i++ )
                    //{
                        if ( tracing != null )
                            tracing.Add( string.Format( "Picking outpost planet #{0}...\n", candidatePlanets.Count+1 ) );

                        bool ratePlanet( Planet p, out PlanetRating rating )
                        {
                            rating = new PlanetRating();
                            if ( p.HasPlanetBeenDestroyed )
                                return false;

                            // minimum 3 hops from another outpost
                            // maximum 6 hops to another outpost
                            foreach ( var p2 in candidatePlanets )
                            {
                                if ( p2.GetHopsTo( p ) <= 3 )
                                    return false;
                                if ( p2.GetHopsTo( p ) > 6 )
                                    return false;
                            }

                            // minimum 3 hops from human home planet
                            if ( p.OriginalHopsToHumanHomeworld <= 3 )
                                return false;

                            // not a bastion or ai homeworld
                            if ( p.PopulationType == PlanetPopulationType.AIHomeworld || p.PopulationType == PlanetPopulationType.AIBastionWorld )
                                return false;

                            // rating based on number of planets we could form links to without causing a planet overlap
                            int r = 0;
                            galaxy.DoForPlanetsSingleThread( false, ( p2 )
                                 =>
                             {
                                 if ( p2.HasPlanetBeenDestroyed )
                                     return DelReturn.Continue;
                                 foreach ( var p3 in candidatePlanets )
                                 {
                                     if ( p3.GetHopsTo( p2 ) <= 3 )
                                         return DelReturn.Continue;
                                     if ( p3.GetHopsTo( p2 ) > 6 )
                                         return DelReturn.Continue;
                                 }
                                 if ( p2.OriginalHopsToHumanHomeworld <= 3 )
                                     return DelReturn.Continue;
                                 if ( p2.PopulationType == PlanetPopulationType.AIHomeworld || p.PopulationType == PlanetPopulationType.AIBastionWorld )
                                     return DelReturn.Continue;

                                 if ( !galaxy.CheckForOverlapWithExistingLines( p, p.GalaxyLocation, p2, p2.GalaxyLocation, true, true ) )
                                     r += 100;

                                 return DelReturn.Continue;
                             } );
                            rating.Rating = (uint)r;

                            return true;
                        }
                        debugStage = 300;
                        var p1 = galaxy.GetRandomPlanet( context, ratePlanet, tracing );
                        debugStage = 400;
                        if ( p1 == null )
                        {
                            if ( tracing != null )
                                tracing.Add( string.Format( "Failed picking candidate...\n" ) );
                        }
                        else
                        {
                            if ( tracing != null )
                                tracing.Add( string.Format( "Picked candidate {0}.\n", p1.Name ) );
                            candidatePlanets.Add( p1 );
                        }
                        debugStage = 450;
                    //}

                    // retry if total planet overlaps from the new links
                    // is higher than some threshold

                    if ( tracing != null )
                        tracing.Add( string.Format( "Check for overlaps...\n" ) );

                    debugStage = 500;
                    int overlaps = 0;
                    galaxy.DoForPlanetsSingleThread( false, ( p )
                         =>
                     {
                         if ( candidatePlanets.Contains( p ) )
                             return DelReturn.Continue;

                         // does this planet overlap one of the links between candidates
                         {
                             Planet last = null;
                             for (int i = 0; i < candidatePlanets.Count; i++)
                             {
                                 var a = candidatePlanets[i];
                                 if (last != null)
                                 {
                                     var b = last;

                                     if ( galaxy.CheckForTooCloseToLine( p, a.GalaxyLocation, b.GalaxyLocation, true ) )
                                     {
                                         if ( tracing != null )
                                             tracing.Add( string.Format( "'{0}' overlaps outpost '{1}'->'{2}'\n", p.Name, a.Name, b.Name ) );
                                         overlaps++;
                                     }
                                 }

                                 last = a;
                             }

                             if (candidatePlanets.Count > 2)
                             {
                                 var a = candidatePlanets[0];
                                 var b = last;

                                 if ( galaxy.CheckForTooCloseToLine( p, a.GalaxyLocation, b.GalaxyLocation, true ) )
                                 {
                                     if ( tracing != null )
                                         tracing.Add( string.Format( "'{0}' overlaps outpost '{1}'->'{2}'\n", p.Name, a.Name, b.Name ) );
                                     overlaps++;
                                 }
                             }
                         }

                         return DelReturn.Continue;
                     } );

                    if ( tracing != null )
                        tracing.Add( string.Format( "Total overlaps '{0}'...\n", overlaps ) );

                    if ( overlaps < 3 && p1 != null )
                        break;
                    retries++;
                }

                if (retries == 5)
                {
                    BaseInfo.LastOutpostFailed = true;

                    ArcenDebugging.ArcenDebugLogSingleLine(
                            string.Format( "[claim] failed after retries to create an outpost anywhere" ),
                            Verbosity.DoNotShow );
                }
                else
                {
                    foreach ( var p in candidatePlanets )
                    {
                        var deepOutpost = p.GetDeepOutpost();
                        if ( deepOutpost == null )
                        {
                            var outpostPos = p.GetSafePlacementPointAroundPlanetCenter( context, Reclaimers.DeepOutpostType,
                                FInt.FromParts( 0, 850 ), FInt.FromParts( 0, 950 ) );
                            var planetFaction = p.GetPlanetFactionForFaction( AttachedFaction );

                            deepOutpost = GameEntity_Squad.CreateNew_ReturnNullIfMPClient( planetFaction, Reclaimers.DeepOutpostType, 1,
                                AttachedFaction.LooseFleet, 0, outpostPos, context, "Reclaimers-BuildOutpost" );
                            var newData = deepOutpost.CreateExternalBaseInfo<ReclaimersDeepSpaceOutpostPerUnitBaseInfo>( "ReclaimersDeepSpaceOutpostPerUnitBaseInfo" );

                            ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format( "[claim] created new deep space outpost on '{0}'", p.Name ),
                                Verbosity.DoNotShow );

                            SquadViewChatHandlerBase clickHandler = ChatClickHandler.CreateNewAs<SquadViewChatHandlerBase>( "ShipGeneralFocus" );
                            if ( clickHandler != null )
                                clickHandler.SquadToView = LazyLoadSquadWrapper.Create( deepOutpost );

                            debugStage = 80;

                            ArcenCharacterBuffer buffer = ArcenCharacterBuffer.GetFromPoolOrCreate( "Reclaimers_ChatMessage" );
                            buffer.Add( "<color=#" ).Add(AttachedFaction.FactionTrimColor.ColorHex).Add(">").Add(AttachedFaction.GetDisplayName()).Add("</color> just created a new ").Add(deepOutpost.GetTypeDisplayNameSafe()).Add(" on ").AddPlanetNameFormated( p, false );

                            World_AIW2.Instance.QueueChatMessageOrCommand( buffer.ToStringAndReturnToPool(), ChatType.LogToCentralChat, "AnyEnemiesAlarm", clickHandler );

                            BaseInfo.LastDeepOutpost = SafeSquadWrapper.Create(deepOutpost);

                            // also give player vision
                            //p.GrantIntel( PlanetIntelLevel.ExploredByDistantHacking );
                            //p.GameSecondLastHadVisionBase = World_AIW2.Instance.GameSecond;
                        }

                        bool linksChanged = false;

                        foreach ( var p2 in candidatePlanets )
                        {
                            if ( p2 == p )
                                continue;

                            GameEntity_Squad e = p2.GetDeepOutpost();
                            if ( e == null )
                                continue;

                            if ( p.GetIsDirectlyLinkedTo( false, p2 ) )
                            {
                                ArcenDebugging.ArcenDebugLogSingleLine(
                                    string.Format( "[claim] {0} and {1} already linked", p.Name, p2.Name ),
                                    Verbosity.DoNotShow );
                                continue;
                            }

                            ArcenPoint posa, posb;
                            posa = deepOutpost.WorldLocation;
                            posb = e.WorldLocation;

                            // create a wormhole linking this to all other outpost planets
                            var cmd = GameCommand.Create( BaseGameCommand.CommandsByCode[BaseGameCommand.Code.LinkPlanets], GameCommandSource.AnythingElse );
                            cmd.RelatedIntegers.Add( p.Index );
                            cmd.RelatedIntegers.Add( p2.Index );
                            cmd.RelatedPoints.Add( posa );
                            cmd.RelatedPoints.Add( posb );
                            cmd.RelatedString = "Claim_ArtificalWormhole";
                            World_AIW2.Instance.QueueGameCommand( this.AttachedFaction, cmd, false );

                            linksChanged = true;

                            ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format( "[claim] linking '{0}' and '{1}'", p.Name, p2.Name ),
                                Verbosity.DoNotShow );
                        }

                        if (linksChanged)
                        {
                            World_AIW2.Instance.CurrentGalaxy.RecomputeDestinationIndexToWormholeMapping();
                            World_AIW2.Instance.CurrentGalaxy.RecomputePlanetDistances();
                            World_AIW2.Instance.CurrentGalaxy.RecomputeLinkedPathfindables();
                        }
                    }
                }

                Planet.ReleaseTemporaryPlanetList(candidatePlanets);
                candidatePlanets = null;
            }
            catch ( Exception e )
            {
                ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format( "debugStage {0}\n{1}", debugStage, e ),
                                Verbosity.ShowAsError );

                if (candidatePlanets != null)
                    Planet.ReleaseTemporaryPlanetList(candidatePlanets);
            }
        }

        private void RemoveDeepOutpost(ArcenHostOnlySimContext context, ArcenCharacterBuffer tracing)
        { 
            int debugStage = 0;

            List<SafeSquadWrapper> drawBag = null;
            try
            {
                if (DeepSpaceOutpostsLRP.Count == 0)
                {
                    if ( tracing != null )
                        tracing.Add( string.Format( "Cant remove outpost none exist.\n" ) );
                    return;
                }

                debugStage = 100;

                drawBag = GameEntity_Squad.GetTemporarySquadList("ExodianBladeFactionDeepInfo-drawBag", 10);
                for (int i = 0; i < DeepSpaceOutpostsLRP.Count; i++)
                {
                    var outpost = DeepSpaceOutpostsLRP[i];
                    var tmp = outpost.GetSquad();
                    if (tmp == null)
                        continue;
                    if (tmp == BaseInfo.LastDeepOutpost.GetSquad() && DeepSpaceOutpostsLRP.Count > 1)
                        continue;

                    drawBag.Add(tmp);
                }

                var tmp1 = drawBag[context.RandomToUse.Next(drawBag.Count)];
                var squad1 = tmp1.GetSquad();
                if ( squad1 == null )
                { 
                    if ( tracing != null )
                        tracing.Add( string.Format( "Tried removing an outpost but it was null.\n" ) );
                    return;
                }

                if ( tracing != null )
                    tracing.Add( string.Format( "We are removing outpost at {0}.\n", squad1.Planet.Name ) );

                var p1 = tmp1.Planet;
                
                for (var i = 0; i < DeepSpaceOutpostsLRP.Count; i++)
                {
                    var tmp2 = DeepSpaceOutpostsLRP[i];
                    var squad2 = tmp2.GetSquad();
                    if (squad2 == null)
                        continue;

                    if (squad2 == squad1)
                        continue;

                    var p2 = squad2.Planet;
                    if (p2.GetIsDirectlyLinkedTo(false, p1))
                    {
                        var cmd = GameCommand.Create(
                            BaseGameCommand.CommandsByCode[BaseGameCommand.Code.UnlinkPlanets],
                            GameCommandSource.AnythingElse );
                        cmd.RelatedIntegers.Add( p1.Index );
                        cmd.RelatedIntegers.Add( p2.Index );
                        World_AIW2.Instance.QueueGameCommand( this.AttachedFaction, cmd, true );

                        ArcenDebugging.ArcenDebugLogSingleLine(
                            string.Format( "[claim] unlinking '{0}' and '{1}'", p1.Name, p2.Name ),
                            Verbosity.DoNotShow );

                        World_AIW2.Instance.CurrentGalaxy.RecomputeDestinationIndexToWormholeMapping();
                        World_AIW2.Instance.CurrentGalaxy.RecomputePlanetDistances();
                        World_AIW2.Instance.CurrentGalaxy.RecomputeLinkedPathfindables();
                    }
                }

                squad1.Despawn(context, true, InstancedRendererDeactivationReason.DespawnsAfterXSeconds);
            }
            catch ( Exception e )
            {
                ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format( "debugStage {0}\n{1}", debugStage, e ),
                                Verbosity.ShowAsError );
            }
            finally
            {
                if (drawBag != null)
                    GameEntity_Squad.ReleaseTemporarySquadList(drawBag);
            }
        }

        public void SendShipToPlanet( GameEntity_Squad entity, Planet dstPlanet, ArcenHostOnlySimContext context, PerFactionPathCache pathingCacheData )
        {
            if (entity.Planet != dstPlanet)
            {
                PathBetweenPlanetsForFaction pathCache = PathingHelper.FindPathFreshOrFromCache(
                    entity.PlanetFaction.Faction, "Claim_SendShipToPlanet",
                    entity.Planet, dstPlanet, PathingMode.Safest, context, pathingCacheData);
                if (pathCache != null && pathCache.PathToReadOnly.Count > 0)
                {
                    GameCommand command = GameCommand.Create(
                        BaseGameCommand.CommandsByCode[BaseGameCommand.Code.SetWormholePath_NPCSingleUnit],
                        GameCommandSource.AnythingElse);
                    command.RelatedString = "Claim_Dest";
                    command.RelatedEntityIDs.Add(entity.PrimaryKeyID);
                    for (int k = 0; k < pathCache.PathToReadOnly.Count; k++)
                        command.RelatedIntegers.Add(pathCache.PathToReadOnly[k].Index);
                    World_AIW2.Instance.QueueGameCommand(this.AttachedFaction, command, false);
                }
            }
        }

        public void SendShipToLocation( GameEntity_Squad entity, Planet dstPlanet, ArcenPoint dstPos, ArcenHostOnlySimContext context, PerFactionPathCache pathingCacheData )
        {
            SendShipToPlanet(entity, dstPlanet, context, pathingCacheData);

            GameCommand moveCommand = GameCommand.Create( BaseGameCommand.CommandsByCode[BaseGameCommand.Code.MoveManyToOnePoint_NPCVisitTargetOnPlanet], GameCommandSource.AnythingElse );
            moveCommand.PlanetOrderWasIssuedFrom = dstPlanet.Index;
            moveCommand.RelatedPoints.Add( dstPos );
            moveCommand.RelatedEntityIDs.Add( entity.PrimaryKeyID );
            World_AIW2.Instance.QueueGameCommand( this.AttachedFaction, moveCommand, false );
        }

        public void SeedSpecialStuff( Galaxy galaxy, ArcenHostOnlySimContext _context, MapTypeData mapType)
        {
            int debugCode = 0;
            try
            {
                // Ignore the context we are passed
                // We want this to occur deterministically based on the map seed
                ArcenHostOnlySimContext context = NonThreadBasedArcenClientOrHostSimContext.GetFromPoolOrCreate( "Claim_SeedSpecialStuff" ).GetHostOnlyContext();
                context.RandomToUse.ReinitializeWithSeed(World_AIW2.Instance.Setup.MapConfig.Seed);

                int numResearchOutposts = AttachedFaction.Config.GetIntValueForCustomFieldOrDefaultValue("NumResearchOutposts",false);
                int numCapturables = AttachedFaction.Config.GetIntValueForCustomFieldOrDefaultValue("NumResearchOutposts",false);

                debugCode = 1;

                int numBST = 0;
                int numODS = 1;
                if (numResearchOutposts > 2)
                    numBST = 1;
                int numFleet = numResearchOutposts - numBST - numODS;
                if (numResearchOutposts > 5)
                    numBST = 2;

                int numFactionsThatARSHack = 0;
                int numFactionsThatTSSHack = 0;
                int numFactionsThatODSHack = 0;
                int numFactionsThatSeedOfficers = 0;
                string officerTag = "AnyOfficer";
                int numFactionsThatSeedMobileStrike = 0;
                string mobileStrikeFleetTag = "RegularStrike";

                debugCode = 2;

                foreach ( Faction player in World_AIW2.Instance.AllPlayerFactions )
                {
                    PlayerTypeData playerType = player.PlayerTypeDataOrNull_ModeratelyExpensive;
                    if ( playerType == null )
                        continue;
                    if ( playerType.MapGen_ShouldARSesSeedForMe )
                        numFactionsThatARSHack++;
                    if ( playerType.MapGen_ShouldTSSesSeedForMe )
                        numFactionsThatTSSHack++;
                    if ( playerType.MapGen_ShouldODSSesSeedForMe )
                        numFactionsThatODSHack++;
                    if ( playerType.MapGen_ShouldOfficerFleetsSeedForMe )
                    {
                        numFactionsThatSeedOfficers++;

                        if ( playerType.MapGen_RequireOfficerFleetTag != null && playerType.MapGen_RequireOfficerFleetTag.Length > 0 )
                            officerTag = playerType.MapGen_RequireOfficerFleetTag;
                    }
                    if ( playerType.MapGen_ShouldMobileStrikeFleetsSeedForMe )
                    {
                        numFactionsThatSeedMobileStrike++;

                        if ( playerType.MapGen_RequireMobileStrikeFleetTag != null && playerType.MapGen_RequireMobileStrikeFleetTag.Length > 0 )
                            mobileStrikeFleetTag = playerType.MapGen_RequireMobileStrikeFleetTag;
                    }
                }

                debugCode = 3;

                if (AIWar2GalaxySettingTable.GetIsBoolSettingEnabledByName_DuringGame("ExpertOfficers"))
                    officerTag += "_Expert";

                ThrowawayListCanMemLeak<Planet> list = null;

                debugCode = 4;

                if (numFactionsThatARSHack > 0)
                {
                    list = StandardMapPopulator.Mapgen_SeedSpecialEntities( context, galaxy, AttachedFaction, SpecialEntityType.None, "ReclaimersOutpost", SeedingType.HardcodedCount, numFleet,
                        MapGenCountPerPlanet.One, MapGenSeedStyle.BigGood, 3, 1, PlanetSeedingZone.OuterSystem, SeedingExpansionType.ComplicatedOriginal, null, 4 );
                    /*
                    foreach (var p in list)
                    {
                        p.GrantIntel( PlanetIntelLevel.ExploredByDistantHacking );
                        p.GameSecondLastHadVisionBase = World_AIW2.Instance.GameSecond;
                    }
                    */

                    ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] seeded {0} ReclaimersOutpost", list.Count),
                                Verbosity.DoNotShow);
                }

                debugCode = 5;

                if (numFactionsThatTSSHack > 0)
                {
                    list = StandardMapPopulator.Mapgen_SeedSpecialEntities( context, galaxy, AttachedFaction, SpecialEntityType.None, "ReclaimersOutpost_BST", SeedingType.HardcodedCount, numBST,
                        MapGenCountPerPlanet.One, MapGenSeedStyle.BigGood, 3, 1, PlanetSeedingZone.OuterSystem, SeedingExpansionType.ComplicatedOriginal, null, 4 );
                    /*
                    foreach (var p in list)
                    {
                        p.GrantIntel( PlanetIntelLevel.ExploredByDistantHacking );
                        p.GameSecondLastHadVisionBase = World_AIW2.Instance.GameSecond;
                    }
                    */

                     ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] seeded {0} ReclaimersOutpost_BST", list.Count),
                                Verbosity.DoNotShow);
                }

                debugCode = 6;

                if (numFactionsThatODSHack > 0)
                {
                    list = StandardMapPopulator.Mapgen_SeedSpecialEntities( context, galaxy, AttachedFaction, SpecialEntityType.None, "ReclaimersOutpost_ODS", SeedingType.HardcodedCount, numODS,
                        MapGenCountPerPlanet.One, MapGenSeedStyle.BigGood, 3, 1, PlanetSeedingZone.OuterSystem, SeedingExpansionType.ComplicatedOriginal, null, 4 );
                    /*
                    foreach (var p in list)
                    {
                        p.GrantIntel( PlanetIntelLevel.ExploredByDistantHacking );
                        p.GameSecondLastHadVisionBase = World_AIW2.Instance.GameSecond;
                    }
                    */

                    ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] seeded {0} ReclaimersOutpost_ODS", list.Count),
                                Verbosity.DoNotShow);
                }

                debugCode = 7;

                if (numFactionsThatARSHack > 0)
                {
                    list = StandardMapPopulator.Mapgen_SeedSpecialEntities( context, galaxy, AttachedFaction, SpecialEntityType.None, "ReclaimersScrapyard", SeedingType.HardcodedCount, 1,
                        MapGenCountPerPlanet.One, MapGenSeedStyle.SmallGood, 3, 1, PlanetSeedingZone.OuterSystem, SeedingExpansionType.DoNoExpansion, null, 4 );
                    /*
                    foreach (var p in list)
                    {
                        p.GrantIntel( PlanetIntelLevel.ExploredByDistantHacking );
                        p.GameSecondLastHadVisionBase = World_AIW2.Instance.GameSecond;
                    }
                    */

                    ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] seeded {0} ReclaimersScrapyard", list.Count),
                                Verbosity.DoNotShow);
                }

                debugCode = 8;

                var naturalFaction = World_AIW2.Instance.GetNaturalObjectFactionNeverNull();

                // For every officer seeded there is a 5% chance of a Chromatic Ark, with a maximum of 25%
                // For every mobile strike seeded there is a 2% chance of a Jump Transport, with a maximum of 40%
                // These are mutually exclusive.
                // 
                bool seededJumpArk = false;
                if (numFactionsThatSeedOfficers > 0)
                {
                    var officers = GameEntity_Squad.GetTemporaryUnsafeSquadList("Reclaimers.officers", 5.0f);

                    naturalFaction.DoForEntities(
                        (GameEntity_Squad e)=>
                        {
                            officers.Add(e);
                            return DelReturn.Continue;
                        }, officerTag );
                
                    int chanceJumpArk = 5 * officers.Count;
                    if (chanceJumpArk > 25)
                        chanceJumpArk = 25;

                    ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] found {0} officers tagged with {1} so there is a {2} chance of a ChromaticArk.", officers.Count, officerTag, chanceJumpArk),
                                Verbosity.DoNotShow);

                    if (context.RandomToUse.Next(0, 100) < chanceJumpArk)
                    {
                        ArcenArrays.Randomize(officers, context.RandomToUse);

                        while (officers.Count > 0)
                        {
                            var officer = officers[0];
                            officers.RemoveAt(0);

                            // Avoid replacing Golems, just replace Arks.
                            if (!officer.TypeData.IsArk)
                                continue;

                            ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] replacing officer {0} on {1} with a ChromaticArk", officer.GetTypeDisplayNameSafe(), officer.GetPlanetNameSafe()),
                                Verbosity.DoNotShow);

                            officer.TransformInto(context, Reclaimers.ChromaticArkType, 1, false);

                            seededJumpArk = true;

                            break;
                        }
                    }

                    GameEntity_Squad.ReleaseTemporaryUnsafeSquadList(officers);
                }

                debugCode = 9;

                if (seededJumpArk)
                {
                    ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] skipped seeding JumpTransport because already seeded JumpArk."),
                                Verbosity.DoNotShow);
                }
                else if (numFactionsThatSeedMobileStrike > 0)
                {
                    var transports = GameEntity_Squad.GetTemporaryUnsafeSquadList("Reclaimers.transports", 5.0f);

                    debugCode = 10;

                    naturalFaction.DoForEntities(
                        (GameEntity_Squad e)=>
                        {
                            transports.Add(e);
                            return DelReturn.Continue;
                        }, mobileStrikeFleetTag );
                
                    int chanceJumpTransport = 2 * transports.Count;
                    if (chanceJumpTransport > 40)
                        chanceJumpTransport = 40;

                    ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] found {0} transports tagged with {1} so there is a {2} chance of a JumpTransport.", transports.Count, mobileStrikeFleetTag, chanceJumpTransport),
                                Verbosity.DoNotShow);

                    debugCode = 11;

                    if (context.RandomToUse.Next(0, 100) < chanceJumpTransport)
                    {
                        ArcenArrays.Randomize(transports, context.RandomToUse);

                        while (transports.Count > 0)
                        {
                            var transport = transports[0];
                            transports.RemoveAt(0);

                            debugCode = 11;

                            // Avoid replacing special transports, just replace basic ones.
                            if (transport.TypeData.DisplayName != "Transport Flagship")
                                continue;

                            ArcenDebugging.ArcenDebugLogSingleLine(
                                string.Format("[claim] replacing transport {0} on {1} with a JumpTransport", transport.GetTypeDisplayNameSafe(), transport.GetPlanetNameSafe()),
                                Verbosity.DoNotShow);

                            debugCode = 12;

                            transport.TransformInto(context, Reclaimers.JumpTransportType, 1, false);

                            debugCode = 13;

                            break;
                        }
                    }

                    GameEntity_Squad.ReleaseTemporaryUnsafeSquadList(transports);
                }

                debugCode = 20;
            }
            catch (Exception e)
            {
                ArcenDebugging.ArcenDebugLogSingleLine(string.Format("claim: debugCode {0}\n{1}", debugCode, e), Verbosity.ShowAsError);
            }
        }
    }
}

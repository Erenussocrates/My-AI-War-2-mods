using System;
using System.Linq;
using System.Runtime.InteropServices;
using Arcen.AIW2.Core;
using Arcen.AIW2.External;
using Arcen.Universal;

namespace ExodianBlade
{
    public class ExodianBladeFactionBaseInfo : ExternalFactionBaseInfoRoot, IExternalBaseInfo_Singleton
    {
        // serialized
        public int BladeAtPlanetIndex;

        //not serialized
        // nothing yet

        public ExodianBladeFactionBaseInfo()
        {
            LOG.Msg("{0}() called.", this.TypeNameAndMethod());
            Cleanup();
        }

        protected override void DoAnySubInitializationImmediatelyAfterFactionAssigned()
        {
        }

        protected override void Cleanup()
        {
            BladeAtPlanetIndex = -1;
        }

        public override void DeserializeFactionIntoSelf(SerMetaData MetaData, ArcenDeserializationBuffer Buffer, SerializationCommandType SerializationCmdType)
        {
            BladeAtPlanetIndex = Buffer.ReadInt32(MetaData, ReadStyle.PosExceptNeg1, "ExodianBlade.BladeAtPlanetIndex");
        }

        public override void SerializeFactionTo(SerMetaData MetaData, ArcenSerializationBuffer Buffer, SerializationCommandType SerializationCmdType)
        {
            Buffer.AddInt32(MetaData, ReadStyle.PosExceptNeg1, BladeAtPlanetIndex, "ExodianBlade.BladeAtPlanetIndex");
        }

        public override float CalculateYourPortionOfPredictedGameLoad_Where100IsANormalAI(ArcenCharacterBufferBase OptionalExplainCalculation)
        {
            if (OptionalExplainCalculation != null)
                OptionalExplainCalculation.Add("No freaking clue.");
            return 0.0f;
        }

        public override int GetDifficultyOrdinal_OrNegativeOneIfNotRelevant()
        {
            return -1;
        }

        public override void SetStartingFactionRelationships()
        {
            base.SetStartingFactionRelationships();
        }

        protected override void DoFactionGeneralAggregationsPausedOrUnpaused()
        {
        }

        protected override void DoRefreshFromFactionSettings()
        {
        }

        public override void DoPerSecondLogic_Stage3Main_OnMainThreadAndPartOfSim_ClientAndHost(ArcenClientOrHostSimContextCore Context)
        {
            if (!Context.ShouldThisMethodBeSkippedBecauseThisMachineIsClient())
            {
            }
            base.DoPerSecondLogic_Stage3Main_OnMainThreadAndPartOfSim_ClientAndHost(Context);
        }

        public override void DoPerSecondLogic_Stage2Aggregating_OnMainThreadAndPartOfSim_ClientAndHost(ArcenClientOrHostSimContextCore Context)
        {
            int debugCode = 0;
            try
            {
                // stuff
            }
            catch (Exception e)
            {
                ArcenDebugging.ArcenDebugLogSingleLine("Hit exception debugCode " + debugCode + " in reclaimers stage 2 " + e.ToString(), Verbosity.DoNotShow);
            }
        }

        public override void AppendStateForDebugDisplay(ArcenCharacterBufferBase buffer)
        {
            //For debug, this goes in the Threat menu
        }

        public override void WriteFactionSlotStatus(ArcenCharacterBufferBase buffer)
        {
            buffer.Add("<i>");
            buffer.AddFactionColoredString("Feel My Blade (heh heh heh).", AttachedFaction);
            buffer.Add("</i>");
        }
    }
}

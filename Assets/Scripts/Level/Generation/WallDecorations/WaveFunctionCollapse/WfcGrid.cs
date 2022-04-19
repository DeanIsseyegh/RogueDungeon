using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Level.Generation.WallDecorations.WaveFunctionCollapse
{
    public class WfcGrid
    {
        public List<WallDecoration> WeightedDecos { get; }
        public List<WallDecoration> UnweightedDecos { get; private set; }
        public WallDecoration DefaultDeco { get; }

        public bool IsCollapsed { get; private set; }
        private WfcGrid _leftNeighbour;
        private WfcGrid _rightNeighbour;
        public WallDecoration CollapsedValue { get; private set; }

        public WfcGrid(List<WallDecoration> weightedDecos, List<WallDecoration> unweightedDecos,
            WallDecoration defaultDeco)
        {
            WeightedDecos = weightedDecos;
            UnweightedDecos = unweightedDecos;
            DefaultDeco = defaultDeco;
        }

        public void LinkLeftNeighbour(WfcGrid leftNeighbour)
        {
            _leftNeighbour = leftNeighbour;
        }

        public void LinkRightNeighbour(WfcGrid rightNeighbour)
        {
            _rightNeighbour = rightNeighbour;
        }

        public void RandomCollapse()
        {
            CollapseBasedOnNeighbour(_leftNeighbour);
            CollapseBasedOnNeighbour(_rightNeighbour);
            if (UnweightedDecos.Count == 0)
            {
                UnweightedDecos.Add(DefaultDeco);
                CollapsedValue = DefaultDeco;
                IsCollapsed = true;
            }
            else
            {
                int randomIndex = Random.Range(0, WeightedDecos.Count);
                CollapsedValue = WeightedDecos[randomIndex];
                UnweightedDecos = UnweightedDecos
                    .Where(it => it.decoWallDecoName == CollapsedValue.decoWallDecoName).ToList();
                IsCollapsed = true;
            }

            _leftNeighbour?.Collapse();
            _rightNeighbour?.Collapse();
        }

        public void Collapse()
        {
            if (IsCollapsed) return;
            bool shouldPropagateToLeft = CollapseBasedOnNeighbour(_leftNeighbour);
            bool shouldPropagateToRight = CollapseBasedOnNeighbour(_rightNeighbour);
            if (shouldPropagateToLeft) _leftNeighbour.Collapse();
            if (shouldPropagateToRight) _rightNeighbour.Collapse();
        }

        private bool CollapseBasedOnNeighbour(WfcGrid neighbour)
        {
            // Remove currentGridValues based on left/right neighbour 
            if (neighbour == null) return false;
            List<WallDecoration> decosToRemove = new List<WallDecoration>();
            for (var i = 0; i < UnweightedDecos.Count; i++)
            {
                var deco = UnweightedDecos[i];
                bool isDecoAllowed = neighbour.IsDecoAllowedAccordingToNeighbour(deco);
                if (!isDecoAllowed) decosToRemove.Add(deco);
            }

            // If grid value has changed, then propagate to neighbour
            if (!decosToRemove.IsNullOrEmpty())
            {
                UnweightedDecos.RemoveAll(it => decosToRemove.Contains(it));
                WeightedDecos.RemoveAll(it => decosToRemove.Contains(it));
                if (UnweightedDecos.Count == 1)
                {
                    IsCollapsed = true;
                    CollapsedValue = UnweightedDecos.First();
                }

                return true;
            }

            return false;
        }

        public bool IsDecoAllowedAccordingToNeighbour(WallDecoration decoToCheckAgainst)
        {
            for (var i = 0; i < UnweightedDecos.Count; i++)
            {
                var deco = UnweightedDecos[i];
                List<WallDecoName> allowedNeighbours = deco.allowedNeighbours;
                bool isAllowed = allowedNeighbours.First() == WallDecoName.ANYTHING ||
                                 allowedNeighbours.Contains(decoToCheckAgainst.decoWallDecoName);
                if (isAllowed) return true;
            }

            return false;
        }
    }
}
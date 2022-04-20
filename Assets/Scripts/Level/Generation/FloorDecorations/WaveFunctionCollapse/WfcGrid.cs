using System.Collections.Generic;
using System.Linq;
using Level.Generation.WallDecorations;
using Sirenix.Utilities;
using UnityEngine;

namespace Level.Generation.FloorDecorations.WaveFunctionCollapse
{
public class WfcGrid
    {
        public List<FloorDecoration> WeightedDecos { get; }
        public List<FloorDecoration> UnweightedDecos { get; private set; }
        public FloorDecoration DefaultDeco { get; }

        public bool IsCollapsed { get; private set; }

        private WfcGrid _leftNeighbour;
        private WfcGrid _rightNeighbour;
        private WfcGrid _upNeighbour;
        private WfcGrid _downNeighbour;
        public FloorDecoration CollapsedValue { get; private set; }

        public WfcGrid(List<FloorDecoration> weightedDecos, List<FloorDecoration> unweightedDecos,
            FloorDecoration defaultDeco)
        {
            WeightedDecos = weightedDecos;
            UnweightedDecos = unweightedDecos;
            DefaultDeco = defaultDeco;
        }

        public void LinkLeftNeighbour(WfcGrid neighbour)
        {
            _leftNeighbour = neighbour;
        }

        public void LinkRightNeighbour(WfcGrid neighbour)
        {
            _rightNeighbour = neighbour;
        }
        
        public void LinkUpNeighbour(WfcGrid neighbour)
        {
            _upNeighbour = neighbour;
        }
        
        public void LinkDownNeighbour(WfcGrid neighbour)
        {
            _downNeighbour = neighbour;
        }

        public void RandomCollapse()
        {
            CollapseBasedOnNeighbour(_leftNeighbour);
            CollapseBasedOnNeighbour(_rightNeighbour);
            CollapseBasedOnNeighbour(_upNeighbour);
            CollapseBasedOnNeighbour(_downNeighbour);

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
                    .Where(it => it.decoName == CollapsedValue.decoName).ToList();
                IsCollapsed = true;
            }

            _leftNeighbour?.Collapse();
            _rightNeighbour?.Collapse();
            _upNeighbour?.Collapse();
            _downNeighbour?.Collapse();
        }

        public void Collapse()
        {
            if (IsCollapsed) return;

            if (CollapseBasedOnNeighbour(_leftNeighbour)) _leftNeighbour.Collapse();
            if (CollapseBasedOnNeighbour(_rightNeighbour)) _rightNeighbour.Collapse();
            if (CollapseBasedOnNeighbour(_upNeighbour)) _upNeighbour.Collapse();
            if (CollapseBasedOnNeighbour(_downNeighbour)) _downNeighbour.Collapse();
        }

        private bool CollapseBasedOnNeighbour(WfcGrid neighbour)
        {
            // Remove currentGridValues based on neighbour 
            if (neighbour == null) return false;
            List<FloorDecoration> decosToRemove = new List<FloorDecoration>();
            for (var i = 0; i < UnweightedDecos.Count; i++)
            {
                var deco = UnweightedDecos[i];
                bool isDecoAllowed = neighbour.IsNeighbourDecoAllowed(deco);
                if (!isDecoAllowed) decosToRemove.Add(deco);
            }

            // If grid value has changed, return true to signal propagation is needed
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

        public bool IsNeighbourDecoAllowed(FloorDecoration neighbourDeco)
        {
            for (var i = 0; i < UnweightedDecos.Count; i++)
            {
                var deco = UnweightedDecos[i];
                List<FloorDecoName> allowedNeighbours = deco.allowedNeighbours;
                bool isAllowed = allowedNeighbours.First() == FloorDecoName.ANYTHING ||
                                 allowedNeighbours.Contains(neighbourDeco.decoName);
                if (isAllowed) return true;
            }
            return false;
        }
    }
}
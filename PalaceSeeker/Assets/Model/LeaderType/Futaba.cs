using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Futaba : Leader {

    public Futaba(Player player) : base(player) {

    }

    public override void LeaderEffectStage0() {
        player.ChangeAllUnitStats(Unit.UnitStats.Movement, 1);
    }
}

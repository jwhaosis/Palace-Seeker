using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hifumi : Leader{

    public Hifumi(Player player) : base(player) {

    }

    public override void LeaderEffectStage0() {
        player.ChangeAllUnitStats(Unit.UnitStats.Movement, 2, 0);
        player.ChangeAllUnitStats(Unit.UnitStats.Attack, 5, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Leader {

    protected Player player;

    public Leader(Player player) {
        this.player = player;
    }

    public abstract void LeaderEffectStage0();
}

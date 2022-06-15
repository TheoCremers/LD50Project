using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    public static int DefaultLayer = 0;
    public static int PlayerColliders = 6;
    public static int PlayerProjectiles = 7;
    public static int PlayerMeleeSwings = 8;
    public static int EnemyColliders = 9;
    public static int EnemyHitBoxes = 10;
    public static int EnemyProjectiles = 11;
    public static int EnemyMeleeSwings = 12;
    public static int PlayerHitBox = 13;
    public static int FamiliarColliders = 14;
}

public static class Masks
{
    public static int PlayerColliders = 1 << Layers.PlayerColliders;
    public static int PlayerProjectiles = 1 << Layers.PlayerProjectiles;
    public static int PlayerMeleeSwings = 1 << Layers.PlayerMeleeSwings;
    public static int EnemyColliders = 1 << Layers.EnemyColliders;
    public static int EnemyHitBoxes = 1 << Layers.EnemyHitBoxes;
    public static int EnemyProjectiles = 1 << Layers.EnemyProjectiles;
    public static int EnemyMeleeSwings = 1 << Layers.EnemyMeleeSwings;
    public static int PlayerHitBox = 1 << Layers.PlayerHitBox;
    public static int FamiliarColliders = 1 << Layers.FamiliarColliders;
}

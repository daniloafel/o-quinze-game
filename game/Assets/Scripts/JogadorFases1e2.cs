using UnityEngine;
using System.Collections;

public class JogadorFases1e2 : JogadorBase {

    public override int getPontos(string tagHit){
        if (tagHit.Equals("Item5"))  return 5;
        if (tagHit.Equals("Item10")) return 10;
        if (tagHit.Equals("Item15")) return 15;
        if (tagHit.Equals("Item20")) return 20;
        if (tagHit.Equals("Item30")) return 30;
        if (tagHit.Equals("Obstaculo20")) return -20;
        if (tagHit.Equals("Obstaculo50")) return -50;
        if (tagHit.Equals("Obstaculo1000")) return -1000;
        return 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitaaliKello : KelloPohja
{
    public NumMesh MinuuttiYkköset, MinuuttiKymmenet, TuntiYkköset, TuntiKymmenet;
    
    public override void updatetimedisplay()
    {
        MinuuttiYkköset.number = (int)Min % 10;
        MinuuttiKymmenet.number = (int)(Min%60)/10;
        TuntiYkköset.number = (int)(Hour+DifferenceToUTC)%24;
        TuntiKymmenet.number = (int)((Hour+DifferenceToUTC)%24)/10;
    }
}

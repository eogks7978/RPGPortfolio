using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EtcItem : Item
{

    public EtcItem(int code, int quantity) : base(code, quantity)
    {
        Code = code;
        Quantity = quantity;
    }
}

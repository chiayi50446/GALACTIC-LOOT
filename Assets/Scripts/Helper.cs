/*
 * Source File: Helper.cs
 * Author: Chiayi Lin
 * Student Number: 301448962
 * Date Last Modified: 2025-02-23
 * 
 * Program Description:
 * This program manages helper functions.
 * 
 * Revision History:
 * - 2025-02-23: Add Delay.
 */

using System;
using System.Collections;
using UnityEngine;
public class Helper
{
    public static IEnumerator Delay(Action methodName, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        methodName();
    }
}

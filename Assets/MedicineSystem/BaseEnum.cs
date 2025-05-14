using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum BaseEnum 
{
    granule,//粒
    slice,//片
    box,//盒
    bag,//袋
    bottle,
    粒,片,盒,袋,瓶,支,包,箱,件,块,个,
    ml,g,mg,kg,
}

public enum Useway
{
    口服,
    煎服,
    晚上睡前口服,
    必要时口服,
    冲服,
    含服,
    静脉滴注,
    外用,
    雾化,
    ivdrip40gtt_min,
    ivdrip50gtt_min,
    ivdrip15gtt_min,
    ivdrip60gtt_min,
    im,
}

public enum Times
{
    Once,
    Twice,
    thrice,
    一,
    两,
    三,
}
public enum doseunit
{
    mg,
    g,
    kg,
    ml,
}
public enum RowEnum
{
    display,
    edit,

}

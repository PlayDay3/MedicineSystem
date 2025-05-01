using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
    // Start is called before the first frame update
    public RowEnum RowEnum;
    public  MedicineData medicineData=new MedicineData();

    private void Start()
    {
        this.transform.Find("name")?.GetComponent<Button>()?.onClick.AddListener(() =>
            TableManager.Instance.SetAddbutton(3, 2)
        );
    }
    private void OnDestroy()
    {
        this.transform.Find("name")?.GetComponent<Button>()?.onClick.RemoveAllListeners();
    }

    public void InitializeItem()
    {


        //medicineData = this.GetComponent<MedicineData>();
        if (medicineData != null)
        {
            changeUnit();
            changeNumber();

            if (RowEnum == RowEnum.display)
            {
                TextMeshProUGUI field;
                field = this.transform.Find("name")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(medicineData.MedicineMessage.Name);

                field = this.transform.Find("规格")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(medicineData.MedicineMessage.capacity.ToString());



                field = this.transform.Find("产地")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(medicineData.MedicineMessage.source);

                field = this.transform.Find("批号")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(medicineData.MedicineMessage.signIndex);

                // 将 long 类型 Unix 时间戳转换为 DateTime 并格式化为 yyyy-MM-dd
                field = this.transform.Find("生产日期")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                if (field != null)
                {
                    DateTime productionDate = DateTimeOffset.FromUnixTimeMilliseconds(medicineData.ProductionDate).DateTime;
                    field.SetText(productionDate.ToString("yyyy-MM-dd"));
                }

                field = this.transform.Find("有效期")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                if (field != null)
                {
                    DateTime validityDat = DateTimeOffset.FromUnixTimeMilliseconds(medicineData.validity).DateTime;
                    field.SetText(validityDat.ToString("yyyy-MM-dd"));
                    Debug.Log($"ProductionDate (long): {medicineData.ProductionDate}");
                    Debug.Log($"Validity (long): {medicineData.validity}");
                }

                field = this.transform.Find("原价")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(medicineData.originalCost.ToString());

                field = this.transform.Find("售价")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(medicineData.sellPrice.ToString());

                field = this.transform.Find("库存")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(medicineData.number.ToString());
                //Debug.Log(medicineData.number);
                //this.transform.GetChild(9).GetChild(0).GetComponent<TMP_InputField>().text = medicineData.number.ToString();

                DateTime validityDate = DateTimeOffset.FromUnixTimeMilliseconds(medicineData.validity).DateTime;
                // 计算距离过期的天数
                TimeSpan difference = (validityDate - DateTime.Now);
                field = this.transform.Find("距离过期")?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
                field?.SetText(Mathf.Abs(difference.Days).ToString());

                //设置DropDown数值
                SetUnitDrop();

            }else if (RowEnum == RowEnum.edit)
            {
                TMP_InputField field;
                field = this.transform.Find("name")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.MedicineMessage.Name;


                field = this.transform.Find("规格")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.MedicineMessage.capacity.ToString();
                
                field=this.transform.Find("规格")?.GetChild(1)?.GetComponent<TMP_InputField>();
                if(field) field.text=medicineData.MedicineMessage.dosage.ToString();

                TMP_Dropdown TempDrop = this.transform.Find("规格")?.GetChild(2).GetComponent<TMP_Dropdown>();
                switch (medicineData.MedicineMessage.doseunit.ToString())
                {
                    case "mg":
                        TempDrop.value = 0;
                        break;
                    case "g":
                        TempDrop.value = 1;
                        break;
                    case "kg":
                        TempDrop.value = 2;                       
                        break;
                    case "ml":
                        TempDrop.value = 3;
                        break;
                }
                TempDrop.RefreshShownValue();

                TempDrop= this.transform.Find("规格")?.GetChild(3).GetComponent<TMP_Dropdown>();             
                switch (medicineData.MedicineMessage.minunit.ToString())
                {
                    case "片":
                        TempDrop.value = 0;                      
                        break;
                    case "粒":
                        TempDrop.value = 1;                    
                        break;
                    case "支":
                        TempDrop.value = 2;                       
                        break;
                    case "瓶":
                        TempDrop.value = 3;                       
                        break;
                    case "包":
                        TempDrop.value = 4;
                        break;
                    case "袋":
                        TempDrop.value = 5;
                        break;
                    case "块":
                        TempDrop.value = 6;
                        break;
                    case "盒":
                        TempDrop.value = 7;
                        break;
                    case "ml":
                        TempDrop.value = 8;
                        break;
                    case "g":
                        TempDrop.value = 9;
                        break;
                    case "mg":
                        TempDrop.value = 10;
                        break;
                    case "kg":
                        TempDrop.value = 11;
                        break;
                }
                TempDrop.RefreshShownValue();

                TempDrop = this.transform.Find("规格")?.GetChild(4).GetComponent<TMP_Dropdown>();
                switch (medicineData.MedicineMessage.maxunit.ToString())
                {
                    case "盒":
                        TempDrop.value = 0;
                        break;
                    case "瓶":
                        TempDrop.value = 1;
                        break;
                    case "箱":
                        TempDrop.value = 2;
                        break;
                    case "件":
                        TempDrop.value = 3;
                        break;
                    case "包":
                        TempDrop.value = 4;
                        break;
                }
                TempDrop.RefreshShownValue();

                SetUnitDrop();
                field = this.transform.Find("生产日期")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field != null)
                {
                    DateTime productionDate = DateTimeOffset.FromUnixTimeMilliseconds(medicineData.ProductionDate).DateTime.ToLocalTime();
                    field.text = productionDate.ToString("yyyy-MM-dd");
                    Debug.Log($"生产日期: {productionDate:yyyy-MM-dd}");
                }

                field = this.transform.Find("有效期")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field != null)
                {
                    DateTime validityDat = DateTimeOffset.FromUnixTimeMilliseconds(medicineData.validity).DateTime.ToLocalTime();
                    field.text = validityDat.ToString("yyyy-MM-dd");
                    Debug.Log($"有效期: {validityDat:yyyy-MM-dd}");
                }


                //field = this.transform.Find("用法")?.GetChild(0)?.GetComponent<TMP_InputField>();
                //if (field) field.text = medicineData.usage;

                field = this.transform.Find("产地")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.MedicineMessage.source;
                

                field = this.transform.Find("批号")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.MedicineMessage.signIndex;
                
               
                field = this.transform.Find("原价")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.originalCost.ToString();
                
                field = this.transform.Find("售价")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.sellPrice.ToString();
                

                field = this.transform.Find("库存")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.number.ToString();

                //this.transform.GetChild(9).GetChild(0).GetComponent<TMP_InputField>().text = medicineData.number.ToString();

                // 将 long 类型的 validity 转换回 DateTime
                DateTime validityDate = DateTimeOffset.FromUnixTimeMilliseconds(medicineData.validity).DateTime;
                // 计算距离过期的天数
                TimeSpan difference = validityDate - DateTime.Now;
                field = this.transform.Find("距离过期")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = Mathf.Abs(difference.Days).ToString();

                field = this.transform.Find("添加数量")?.GetChild(0)?.GetComponent<TMP_InputField>();
                if (field) field.text = medicineData.addnumber.ToString();

                TempDrop = this.transform.Find("服用次数")?.GetChild(1).GetComponent<TMP_Dropdown>();
                TempDrop.onValueChanged.RemoveAllListeners();
                switch (medicineData.Times.ToString())
                {
                    case "一":
                        TempDrop.value = 0;
                        break;
                    case "两":
                        TempDrop.value = 1;
                        break;
                    case "三":
                        TempDrop.value = 2;
                        break;
                }
                TempDrop.RefreshShownValue();
                TempDrop.onValueChanged.AddListener(index => OnValueChange());

                TempDrop = this.transform.Find("用法")?.GetChild(1).GetComponent<TMP_Dropdown>();
                TempDrop.onValueChanged.RemoveAllListeners();
                switch (medicineData.Useway.ToString())
                {
                    case "口服":
                        TempDrop.value = 0;
                        break;
                    case "晚上睡前口服":
                        TempDrop.value = 1;
                        break;
                    case "必要时口服":
                        TempDrop.value = 2;
                        break;
                    case "冲服":
                        TempDrop.value = 3;
                        break;
                    case "外用":
                        TempDrop.value = 4;
                        break;
                    case "静脉滴注":
                        TempDrop.value = 5;
                        break;
                    case "ivdrip15gtt_min":
                        TempDrop.value = 6;
                        break;
                    case "ivdrip40gtt_min":
                        TempDrop.value = 7;
                        break;
                    case "ivdrip50gtt_min":
                        TempDrop.value = 8;
                        break;
                    case "ivdrip60gtt_min":
                        TempDrop.value = 9;
                        break;
                    case "im":
                        TempDrop.value = 10;
                        break;
                }
                TempDrop.RefreshShownValue();
                TempDrop.onValueChanged.AddListener(index => OnValueChange());


            }
            TMP_Dropdown dropdown = this.transform.Find("单位")?.GetChild(0).GetComponent<TMP_Dropdown>();
            dropdown.onValueChanged.RemoveAllListeners();
            switch (medicineData.unit.ToString())
            {
                case "granule"://英文
                    dropdown.value = 0;
                    dropdown.RefreshShownValue();        
                    //medicineData.BoxNumber = medicineData.granuleNumber / int.Parse(medicineData.MedicineMessage.capacity.ToString());                  
                    break;
                case "box":
                    dropdown.value = 1;
                    dropdown.RefreshShownValue();
                    
                    //medicineData.granuleNumber = medicineData.BoxNumber * int.Parse(medicineData.MedicineMessage.capacity.ToString());
                    break;
                case "bag":
                    dropdown.value = 2;
                    dropdown.RefreshShownValue();                    
                    break;
            }
            dropdown.onValueChanged.AddListener(index=> OnValueChange());

        }
    }

     public void returnItem()
    {
        //MedicineData medicineData = this.GetComponent<MedicineData>();
        if (medicineData != null)
        {
            if(this.transform.Find("name"))
            medicineData.MedicineMessage.Name = this.transform.Find("name")?.GetChild(0).GetComponent<TMP_InputField>().text;
            if(this.transform.Find("规格"))
            medicineData.MedicineMessage.capacity = int.Parse(this.transform.Find("规格")?.GetChild(0).GetComponent<TMP_InputField>().text);
            if (this.transform.Find("规格"))
                medicineData.MedicineMessage.dosage = float.Parse(this.transform.Find("规格")?.GetChild(1).GetComponent<TMP_InputField>().text);
           
            if (this.transform.Find("规格"))
            {
                TMP_Dropdown dropdown = this.transform.Find("规格")?.GetChild(2).GetComponent<TMP_Dropdown>();
                switch (dropdown.value)
                {
                    case 0:
                        medicineData.MedicineMessage.doseunit = doseunit.mg;
                        //medicineData.number=medicineData.granuleNumber;

                        break;
                    case 1:
                        medicineData.MedicineMessage.doseunit = doseunit.g;
                        //medicineData.number=medicineData.BoxNumber;

                        break;
                    case 2:
                        medicineData.MedicineMessage.doseunit = doseunit.kg;
                        break;
                    case 3:
                        medicineData.MedicineMessage.doseunit = doseunit.ml;
                        break;

                }
            }

            if (this.transform.Find("规格"))
            {
                TMP_Dropdown dropdown = this.transform.Find("规格")?.GetChild(3).GetComponent<TMP_Dropdown>();
                switch (dropdown.value)
                {
                    case 0:
                        medicineData.MedicineMessage.minunit = BaseEnum.片;
                        //medicineData.number=medicineData.granuleNumber;

                        break;
                    case 1:
                        medicineData.MedicineMessage.minunit = BaseEnum.粒;
                        //medicineData.number=medicineData.BoxNumber;
                        break;
                   case 2:
                        medicineData.MedicineMessage.minunit = BaseEnum.支;
                        break;
                    case 3:
                        medicineData.MedicineMessage.minunit = BaseEnum.瓶;
                        break;
                    case 4:
                        medicineData.MedicineMessage.minunit = BaseEnum.包;
                        break;
                    case 5:
                        medicineData.MedicineMessage.minunit = BaseEnum.袋;
                        break;
                    case 6:
                        medicineData.MedicineMessage.minunit = BaseEnum.块;
                        break;
                    case 7:
                        medicineData.MedicineMessage.minunit = BaseEnum.盒;
                        break;
                    case 8:
                        medicineData.MedicineMessage.minunit = BaseEnum.ml;
                        break;
                    case 9:
                        medicineData.MedicineMessage.minunit = BaseEnum.g;
                        break;
                    case 10:
                        medicineData.MedicineMessage.minunit = BaseEnum.mg;
                        break;
                    case 11:
                        medicineData.MedicineMessage.minunit = BaseEnum.kg;
                        break;
                }
            }
            if (this.transform.Find("规格"))
            {
                TMP_Dropdown dropdown = this.transform.Find("规格")?.GetChild(4).GetComponent<TMP_Dropdown>();
                switch (dropdown.value)
                {
                    case 0:
                        medicineData.MedicineMessage.maxunit = BaseEnum.盒;
                        break;
                    case 1:
                        medicineData.MedicineMessage.maxunit = BaseEnum.瓶;
                        break;
                    case 2:
                        medicineData.MedicineMessage.maxunit = BaseEnum.箱;
                        break;
                    case 3:
                        medicineData.MedicineMessage.maxunit = BaseEnum.件;
                        break;
                    case 4:
                        medicineData.MedicineMessage.maxunit = BaseEnum.包;
                        break;
                }
            }

            float tempEatDose;
            if (this.transform.Find("单次剂量") &&
                float.TryParse(this.transform.Find("单次剂量")?.GetChild(0).GetComponent<TMP_InputField>().text, out tempEatDose))
            {
                medicineData.eatdose = tempEatDose;
            }

            int tempDay;
            if (this.transform.Find("服用天数") &&
                int.TryParse(this.transform.Find("服用天数")?.GetChild(0).GetComponent<TMP_InputField>().text, out tempDay))
            {
                medicineData.Day = tempDay;
            }


            //if (this.transform.Find("用法"))
            //    medicineData.usage = this.transform.Find("用法")?.GetChild(0).GetComponent<TMP_InputField>().text;

            if (this.transform.Find("用法"))
            {
                TMP_Dropdown dropdown = this.transform.Find("用法")?.GetChild(1).GetComponent<TMP_Dropdown>();
                switch (dropdown.value)
                {
                    case 0:
                        medicineData.Useway=Useway.口服;
                        break;
                    case 1:
                        medicineData.Useway = Useway.晚上睡前口服;
                        break;
                    case 2:
                        medicineData.Useway = Useway.必要时口服;
                        break;
                    case 3:
                        medicineData.Useway = Useway.冲服;
                        break;
                    case 4:
                        medicineData.Useway = Useway.外用;
                        break;
                    case 5:
                        medicineData.Useway = Useway.静脉滴注;
                        break;
                    case 6:
                        medicineData.Useway = Useway.ivdrip15gtt_min;
                        break;
                    case 7:
                        medicineData.Useway = Useway.ivdrip40gtt_min;
                        break;
                    case 8:
                        medicineData.Useway = Useway.ivdrip50gtt_min;
                        break;
                    case 9:
                        medicineData.Useway = Useway.ivdrip60gtt_min;
                        break;
                    case 10:
                        medicineData.Useway = Useway.im;
                        break;

                }
            }

            if (this.transform.Find("产地"))
                medicineData.MedicineMessage.source = this.transform.Find("产地")?.GetChild(0).GetComponent<TMP_InputField>().text;
            if (this.transform.Find("批号"))
                medicineData.MedicineMessage.signIndex = this.transform.Find("批号")?.GetChild(0).GetComponent<TMP_InputField>().text;

            Transform productionDateTransform = this.transform.Find("生产日期");

            if (productionDateTransform != null)
            {
                TMP_InputField inputField = productionDateTransform.GetChild(0)?.GetComponent<TMP_InputField>();

                if (inputField != null)
                {
                    string inputDate = inputField.text.Trim(); // 获取文本并去除空格

                    if (!string.IsNullOrEmpty(inputDate) &&
                        DateTime.TryParseExact(inputDate, "yyyy-MM-dd",
                                               System.Globalization.CultureInfo.InvariantCulture,
                                               System.Globalization.DateTimeStyles.None,
                                               out DateTime parsedDate))
                    {
                        // 转换 DateTime 为 Unix 时间戳（毫秒级）
                        medicineData.ProductionDate = new DateTimeOffset(parsedDate).ToUnixTimeMilliseconds();
                    }
                    else
                    {
                        Debug.LogWarning($"输入的生产日期格式无效: \"{inputDate}\"，使用默认值！");
                        medicineData.ProductionDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }
                }
                else
                {
                    Debug.LogWarning("未找到 TMP_InputField 组件！");
                }
            }


            Transform validityTransform = this.transform.Find("有效期");

            if (validityTransform != null)
            {
                TMP_InputField inputField = validityTransform.GetChild(0)?.GetComponent<TMP_InputField>();

                if (inputField != null)
                {
                    string inputDate = inputField.text.Trim(); // 获取文本并去除空格

                    if (!string.IsNullOrEmpty(inputDate) &&
                        DateTime.TryParseExact(inputDate, "yyyy-MM-dd",
                                               System.Globalization.CultureInfo.InvariantCulture,
                                               System.Globalization.DateTimeStyles.None,
                                               out DateTime parsedDate))
                    {
                        // 转换 DateTime 为 Unix 时间戳（毫秒级）
                        medicineData.validity = new DateTimeOffset(parsedDate).ToUnixTimeMilliseconds();
                    }
                    else
                    {
                        Debug.LogWarning($"输入的有效期格式无效: \"{inputDate}\"，使用默认值！");
                        medicineData.validity = DateTimeOffset.MaxValue.ToUnixTimeMilliseconds();
                    }
                }
                else
                {
                    Debug.LogWarning("未找到 TMP_InputField 组件！");
                }
            }


            if (this.transform.Find("原价"))
            {
                TMP_InputField inputField = this.transform.Find("原价")?.GetChild(0).GetComponent<TMP_InputField>();
                string inputText = inputField?.text.Trim(); // 获取文本并去除空格
                if (!string.IsNullOrEmpty(inputText) && float.TryParse(inputText, out float parsedValue))
                {
                    medicineData.originalCost = parsedValue;
                }
                else
                {
                    Debug.LogWarning($"输入的原价格式无效: \"{inputText}\"，使用默认值！");
                    medicineData.originalCost = 0f; // 或者使用其他默认值
                }
            }
            if (this.transform.Find("售价"))
            {
                TMP_InputField inputField = this.transform.Find("售价")?.GetChild(0).GetComponent<TMP_InputField>();
                string inputText = inputField?.text.Trim(); // 获取文本并去除空格
                if (!string.IsNullOrEmpty(inputText) && float.TryParse(inputText, out float parsedValue))
                {
                    medicineData.sellPrice = parsedValue;
                }
                else
                {
                    Debug.LogWarning($"输入的原价格式无效: \"{inputText}\"，使用默认值！");
                    medicineData.sellPrice = 0f; // 或者使用其他默认值
                }
            }
            if (this.transform.Find("单位")) {
                TMP_Dropdown dropdown = this.transform.Find("单位")?.GetChild(0).GetComponent<TMP_Dropdown>();
                switch (dropdown.value)
                {
                    case 0:
                        medicineData.unit = BaseEnum.granule;
                        //medicineData.number=medicineData.granuleNumber;

                        break;
                    case 1:
                        medicineData.unit = BaseEnum.box;
                        //medicineData.number=medicineData.BoxNumber;

                        break;
                    case 2:
                        medicineData.unit = BaseEnum.granule;
                        break;
                }
            }
            if (this.transform.Find("库存")) {
                if (int.TryParse(this.transform.Find("库存")?.GetChild(0).GetComponent<TMP_InputField>().text.ToString(), out int number))
                {
                    Debug.Log(number);
                    medicineData.number = number;
                }
            }
            if (this.transform.Find("添加数量"))
            {
                if (int.TryParse(this.transform.Find("添加数量")?.GetChild(0).GetComponent<TMP_InputField>().text.ToString(), out int Addnumber))
                {
                    if (Addnumber > 0)
                    {
                        medicineData.addnumber = Addnumber;
                    }

                }
            }
            if (this.transform.Find("距离过期")) {
                if (int.TryParse(this.transform.Find("距离过期")?.GetChild(0).GetComponent<TMP_InputField>().text.ToString(), out int day))
                {
                    medicineData.RemainTime = day;
                }
            }

            if (this.transform.Find("服用次数"))
            {
                TMP_Dropdown dropdown = this.transform.Find("服用次数")?.GetChild(1).GetComponent<TMP_Dropdown>();
                switch (dropdown.value)
                {
                    case 0:
                        medicineData.Times = Times.一;
                        break;
                    case 1:
                        medicineData.Times = Times.两;
                        break;
                    case 2:
                        medicineData.Times = Times.三;
                        break;
                }
            }

        }
    }

    //public void SetDropdown()
    //{


    //    TMP_Dropdown dropdown = this.transform.Find("规格")?.GetChild(3).GetComponent<TMP_Dropdown>();
    //    var Temp = this.transform.Find("规格")?.GetChild(4);
    //    switch (dropdown.value)
    //    {
    //        case 0:
    //            medicineData.MedicineMessage.minunit = BaseEnum.片;
    //            //medicineData.number=medicineData.granuleNumber;
    //            Temp.gameObject.SetActive(true);
    //            break;
    //        case 1:
    //            medicineData.MedicineMessage.minunit = BaseEnum.粒;
    //            //medicineData.number=medicineData.BoxNumber;
    //            Temp.gameObject.SetActive(true);
    //            break;
    //        case 2:
    //            medicineData.MedicineMessage.minunit = BaseEnum.支;
    //            Temp.gameObject.SetActive(true);
    //            break;
    //        case 3:
    //            medicineData.MedicineMessage.minunit = BaseEnum.瓶;
    //            Temp.gameObject.SetActive(false);
    //            break;

    //    }

    //}

    public void changeNumber()
    {
        //MedicineData medicineData = this.GetComponent<MedicineData>();
        switch (medicineData.unit)
        {
            case BaseEnum.granule:
                medicineData.granuleNumber = medicineData.number;
               // Debug.Log(medicineData.granuleNumber);
                medicineData.BoxNumber = medicineData.number / int.Parse(medicineData.MedicineMessage.capacity.ToString());
                break;
            case BaseEnum.box:
                medicineData.BoxNumber = medicineData.number;
                medicineData.granuleNumber = medicineData.BoxNumber * int.Parse(medicineData.MedicineMessage.capacity.ToString());
                break;
            case BaseEnum.bag:
                medicineData.granuleNumber = medicineData.number;
                medicineData.BoxNumber = medicineData.number / int.Parse(medicineData.MedicineMessage.capacity.ToString());
                break;
        }
    }


    public void SetUnitDrop()
    {
        TMP_Dropdown UnitDrop = this.transform.Find("单位")?.GetChild(0).GetComponent<TMP_Dropdown>();
        switch (medicineData.MedicineMessage.minunit)
        {
            case BaseEnum.支:
                UnitDrop.options[0].text = "支";
                break;
            case BaseEnum.片:
                UnitDrop.options[0].text = "片";
                break;
            case BaseEnum.瓶:
                UnitDrop.options[0].text = "瓶";
                break;
            case BaseEnum.粒:
                UnitDrop.options[0].text = "粒";
                break;
            case BaseEnum.包:
                UnitDrop.options[0].text = "包";
                break;
            case BaseEnum.袋:
                UnitDrop.options[0].text = "袋";
                break;
        }
        switch (medicineData.MedicineMessage.maxunit)
        {
            case BaseEnum.瓶:
                UnitDrop.options[1].text = "瓶";
                break;
            case BaseEnum.盒:
                UnitDrop.options[1].text = "盒";
                break;
            case BaseEnum.箱:
                UnitDrop.options[1].text = "箱";
                break;
            case BaseEnum.件:
                UnitDrop.options[1].text = "件";
                break;
            case BaseEnum.包:
                UnitDrop.options[1].text = "包";
                break;
        }
        UnitDrop.RefreshShownValue();
    }

    public void changeUnit()
    {
        TMP_Dropdown dropdown = this.transform.Find("单位")?.GetChild(0).GetComponent<TMP_Dropdown>();
        //MedicineData medicineData = this.GetComponent<MedicineData>();
        switch (dropdown.value)
        {
            case 0:
                
                medicineData.number=medicineData.granuleNumber;
                medicineData.unit=BaseEnum.granule;
                break;
            case 1:            
                medicineData.number=medicineData.BoxNumber;
                medicineData.unit= BaseEnum.box;               
                break;

        }
        if (RowEnum == RowEnum.display) {
            this.transform.Find("库存").GetChild(0).GetComponent<TextMeshProUGUI>().text = medicineData.number.ToString();
        }
        else if(RowEnum==RowEnum.edit)
        {
            this.transform.Find("库存").GetChild(0).GetComponent<TMP_InputField>().text = medicineData.number.ToString();
        }


    }

    public void disPlayMessage()
    {
        WindowControl.instance.SetWindowData(medicineData);
    }

    public void ReturnToList()
    {
        returnItem();
        changeNumber();

        TableManager.Instance.EditList(medicineData);
        TableManager.Instance.displayTable();
    }

    public void AddSellList()
    {
        returnItem();
        TableManager.Instance.AddSellList(medicineData);
    }

    public void OnChangeUseWay()
    {
        if (this.transform.Find("用法"))
        {
            TMP_Dropdown dropdown = this.transform.Find("用法")?.GetChild(1).GetComponent<TMP_Dropdown>();
            switch (dropdown.value)
            {
                case 0:
                    medicineData.Useway = Useway.口服;
                    break;
                case 1:
                    medicineData.Useway = Useway.静脉滴注;
                    break;
                case 2:
                    medicineData.Useway = Useway.外用;
                    break;
                case 3:
                    medicineData.Useway = Useway.ivdrip15gtt_min;
                    break;
                case 4:
                    medicineData.Useway = Useway.ivdrip40gtt_min;
                    break;
                case 5:
                    medicineData.Useway = Useway.ivdrip60gtt_min;
                    break;

            }
        }

    }

    public void OnValueChange()
    {

        returnItem();
        changeNumber();
        int TempTimes;
        switch (medicineData.Times)
        {
            case Times.一:
                TempTimes = 1;
                break;
            case Times.两:
                TempTimes = 2;
                break;
            case Times.三: 
                TempTimes = 3;
                break;
            default:
                TempTimes = 0;
                break;
        }
        medicineData.addnumber = (int)((medicineData.eatdose) * TempTimes*medicineData.Day);
        InitializeItem();
        TableManager.Instance.EditList(medicineData);
        TableManager.Instance.displayTable();
    }












}

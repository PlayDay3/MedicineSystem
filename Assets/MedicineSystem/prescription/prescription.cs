using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using UnityEditor.VersionControl;
using System.Collections.Generic;
using TMPro;
using FancyScrollView.Example08;
using System;
using System.Collections;
using UnityEngine.AI;

public class prescription : MonoBehaviour
{
    public static prescription instance;
    public Canvas diagnosisCanvas; // 诊断单的 Canvas
    public Camera uiCamera; // 渲染诊断单的 UI 摄像机
    public int imageWidth = 1920; // 截图宽度
    public int imageHeight = 1080; // 截图高度
    public string savePath = "DiagnosisReport.png"; // 保存路径
    public GameObject BackGround;
    public PrescriptionMessage PrescriptionMessage;
    public string saveDirectory;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        this.gameObject.SetActive(false);
    }


    // 保存诊断单为图片
    private Rect GetScreenRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // 获取 UI 在屏幕上的四个角的坐标
        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        // 计算实际宽高
        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        // 适配 CanvasScaler 影响
        //Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        //if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
        //{
        //    float scaleFactor = canvas.scaleFactor;
        //    width /= scaleFactor;
        //    height /= scaleFactor;
        //}

        // 计算最终的屏幕 Rect
        //width += 50;
        float x = bottomLeft.x;
        float y = Screen.height - topRight.y;  // ReadPixels 需要左下角为起点

        Debug.Log($"Final Rect: x={x}, y={y}, width={width}, height={height}");


        return new Rect(x, y, width, height);
    }

    public static string ReplaceUnderscoreWithSlash(string input)
    {
        return input.Replace("_", "/");
    }
    public void SaveDiagnosisReport()
    {
        StartCoroutine(CaptureUIElement());
    }

    private IEnumerator CaptureUIElement()
    {
        yield return new WaitForEndOfFrame(); // 确保 UI 完全渲染

        // 计算 BackGround 在屏幕上的 Rect
        Rect rect = GetScreenRect(BackGround.GetComponent<RectTransform>());

        // 确保 rect 大小有效
        if (rect.width <= 0 || rect.height <= 0)
        {
            Debug.LogError("Invalid UI size for screenshot!");
            yield break;
        }

        // 创建 Texture2D 进行截图
        Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        // 生成文件名
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"{PrescriptionMessage.Patient.Name}{timestamp}.png";

        // 如果 saveDirectory 为空，则使用默认路径
        if (string.IsNullOrEmpty(saveDirectory))
        {
            saveDirectory = Application.persistentDataPath;
        }

        // 确保目录存在
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        // 组合完整路径
        string fullPath = Path.Combine(saveDirectory, fileName);

        // 保存为 PNG
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log("Diagnosis report saved at: " + fullPath);
    }



    public void Setprescription()
    {
        TMP_InputField Field;
        //清空数据
        InitializedPrescription();
        Field = BackGround.transform.Find("Id").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.PrescriptionId;

        Field = BackGround.transform.Find("姓名").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.Name;

        Field = BackGround.transform.Find("性别").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.Sex;

        Field = BackGround.transform.Find("年龄").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.age;

        Field = BackGround.transform.Find("地址").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.Location;

        Field = BackGround.transform.Find("电话").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.number;

        Field = BackGround.transform.Find("日期").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.Date;

        Field = BackGround.transform.Find("诊断").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.Description;

        Field = BackGround.transform.Find("医师").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Doctor;

        Field = BackGround.transform.Find("审核").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Check;

        Field = BackGround.transform.Find("费用").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.MedicineCost.ToString();

        TextMeshProUGUI TempText = BackGround.transform.Find("费用").GetChild(1).GetComponent<TextMeshProUGUI>();
        TempText.text =$"({PrescriptionMessage.OriginalCost})" ;

        Field = BackGround.transform.Find("挂号费").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.SignCost.ToString();

        Field = BackGround.transform.Find("注射费").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.InjectionCost.ToString();

        Field = BackGround.transform.Find("治疗费").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.TreatmentCost.ToString();

        Field = BackGround.transform.Find("其他").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.OtherCost.ToString();

        Field = BackGround.transform.Find("合计").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.SumCost.ToString();

        Field = BackGround.transform.Find("利润").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Profit.ToString();

        Field = BackGround.transform.Find("体温").GetChild(1).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.T.ToString();

        Field = BackGround.transform.Find("心率").GetChild(1).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.P.ToString();

        Field = BackGround.transform.Find("血压").GetChild(1).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.BP_b.ToString();

        Field = BackGround.transform.Find("血压").GetChild(2).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.BP_p.ToString();

        Field = BackGround.transform.Find("血糖").GetChild(1).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.Patient.Blood_Sugar.ToString();
        TextMeshProUGUI TempPrescription = BackGround.transform.Find("Rx")?.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TempPrescription.text="\n";
        
        foreach(MedicineData message in PrescriptionMessage.MedicineLsit)
        {
            string TempUseway = ReplaceUnderscoreWithSlash(message.Useway.ToString());

            TempPrescription.text += message.MedicineMessage.Name +"    "+message.MedicineMessage.dosage+message.MedicineMessage.doseunit+"*"+
               message.MedicineMessage.capacity + message.MedicineMessage.minunit+"/"+message.MedicineMessage.maxunit+ "      "+$"总计:{message.addnumber}{message.MedicineMessage.minunit}"+ "\n";
            TempPrescription.text += "用法:" + TempUseway + "        " + "单次:" + message.eatdose * message.MedicineMessage.dosage + message.MedicineMessage.doseunit;
            TempPrescription.text += "\t"+$"每日{message.Times}次\t" + message.Day + "天\n" ;


        }



    }

    public void ReturnPrescription()
    {
        TMP_InputField Field;
        Field = BackGround.transform.Find("姓名").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Patient.Name = Field.text;

        Field = BackGround.transform.Find("性别").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Patient.Sex= Field.text;

        Field = BackGround.transform.Find("年龄").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Patient.age = Field.text;

        Field = BackGround.transform.Find("地址").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Patient.Location = Field.text;

        Field = BackGround.transform.Find("电话").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Patient.number = Field.text;

        Field = BackGround.transform.Find("日期").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Patient.Date = Field.text;

        Field = BackGround.transform.Find("诊断").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Patient.Description = Field.text;

        Field = BackGround.transform.Find("医师").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Doctor = Field.text;

        Field = BackGround.transform.Find("审核").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.Check = Field.text;

        Field = BackGround.transform.Find("费用").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.MedicineCost = float.Parse(Field.text);

        Field = BackGround.transform.Find("挂号费").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.SignCost = float.Parse(Field.text);

        Field = BackGround.transform.Find("注射费").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.InjectionCost = float.Parse(Field.text);

        Field = BackGround.transform.Find("治疗费").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.TreatmentCost = float.Parse(Field.text);

        Field = BackGround.transform.Find("其他").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.OtherCost = float.Parse(Field.text);

        Field = BackGround.transform.Find("合计").GetChild(0).GetComponent<TMP_InputField>();
        PrescriptionMessage.SumCost = float.Parse(Field.text);


        Field = BackGround.transform.Find("体温").GetChild(1).GetComponent<TMP_InputField>();
        if (Field.text != "")
        {
            PrescriptionMessage.Patient.T = float.Parse(Field.text);
        }
        else
        {
            PrescriptionMessage.Patient.T = 0;
        }
        Field = BackGround.transform.Find("体重").GetChild(1).GetComponent<TMP_InputField>();
        if (Field.text != "")
        {
            PrescriptionMessage.Patient.Weight = float.Parse(Field.text);
        }
        else
        {
            PrescriptionMessage.Patient.Weight = 0;
        }


        Field = BackGround.transform.Find("心率").GetChild(1).GetComponent<TMP_InputField>();
        if (Field.text != "")
        {
            PrescriptionMessage.Patient.P = float.Parse(Field.text);
        }
        else
        {
            PrescriptionMessage.Patient.P = 0;
        }


        Field = BackGround.transform.Find("血压").GetChild(1).GetComponent<TMP_InputField>();
        if (Field.text != "")
        {
            PrescriptionMessage.Patient.BP_b = float.Parse(Field.text);
        }
        else
        {
            PrescriptionMessage.Patient.BP_b = 0;
        }


        Field = BackGround.transform.Find("血压").GetChild(2).GetComponent<TMP_InputField>();
        if (Field.text != "")
        {
            PrescriptionMessage.Patient.BP_p = float.Parse(Field.text);
        }
        else
        {
            PrescriptionMessage.Patient.BP_p = 0;
        }

        Field = BackGround.transform.Find("血糖").GetChild(1).GetComponent<TMP_InputField>();
        if (Field.text != "")
        {
            PrescriptionMessage.Patient.Blood_Sugar = float.Parse(Field.text);
        }
        else
        {
            PrescriptionMessage.Patient.Blood_Sugar = 0;
        }


        SaveToManager();
        PrescriptManager.instance.SavePrescriptions();//保存PrescriptionList
        MedicineSerializable.instance.SaveData();
    }


    public void ChangeCost()
    {
        TMP_InputField Field;
        Field = BackGround.transform.Find("费用").GetChild(0).GetComponent<TMP_InputField>();
        if(float.TryParse(Field.text,out float temp))
        {
            PrescriptionMessage.MedicineCost = float.Parse(Field.text);

        }
        else
        {
            PrescriptionMessage.MedicineCost = 0;
            Field.text = "0";
        }

        Field = BackGround.transform.Find("挂号费").GetChild(0).GetComponent<TMP_InputField>();
        if (float.TryParse(Field.text,out float a))
        {
            PrescriptionMessage.SignCost = a;
        }
        else
        {
            PrescriptionMessage.SignCost = 0;
            Field.text = "0";
        }

        Field = BackGround.transform.Find("注射费").GetChild(0).GetComponent<TMP_InputField>();
        if (float.TryParse(Field.text, out float b))
        {
            PrescriptionMessage.InjectionCost = b;
        }
        else
        {
            PrescriptionMessage.InjectionCost = 0;
            Field.text = "0";
        }


        Field = BackGround.transform.Find("治疗费").GetChild(0).GetComponent<TMP_InputField>();
        if (float.TryParse(Field.text, out float c))
        {
            PrescriptionMessage.TreatmentCost = c;
        }
        else
        {
            PrescriptionMessage.TreatmentCost = 0;
            Field.text = "0";
        }

        Field = BackGround.transform.Find("其他").GetChild(0).GetComponent<TMP_InputField>();
        if (float.TryParse(Field.text, out float d))
        {
            PrescriptionMessage.OtherCost = d;
        }
        else
        {
            PrescriptionMessage.OtherCost = 0;
            Field.text = "0";
        }
        PrescriptionMessage.SumCost= PrescriptionMessage.MedicineCost+ PrescriptionMessage.SignCost+ PrescriptionMessage.InjectionCost
            +PrescriptionMessage.TreatmentCost+PrescriptionMessage.OtherCost;
        PrescriptionMessage.Profit = PrescriptionMessage.SumCost - PrescriptionMessage.OriginalCost;

        Field = BackGround.transform.Find("合计").GetChild(0).GetComponent<TMP_InputField>();
        Field.text = PrescriptionMessage.SumCost.ToString();
        ReturnPrescription();
        Setprescription();
        PrescriptManager.instance.SavePrescriptions();
    }

    public void SaveToManager()
    {
        PrescriptManager.instance.AddList(PrescriptionMessage);
        if (!PrescriptionMessage.isPaid && PrescriptionMessage.MedicineLsit.Count!=0)
        {
            TableManager.Instance.SubSellList(PrescriptionMessage.MedicineLsit);
            PrescriptionMessage.isPaid = true;

        }

        Example08.instance.GenerateCells(PrescriptManager.instance.PrescriptList, PrescriptManager.instance.PrescriptList.Count);

    }

    public void InitializedPrescription()
    {
        for(int i = 0; i < 7; i++)
        {
            TMP_InputField Field =BackGround.transform.GetChild(i).GetChild(0).GetComponent<TMP_InputField>();
            Field.text = "";
        }
        TextMeshProUGUI TempPrescription = BackGround.transform.Find("Rx")?.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TempPrescription.text = "";
        TempPrescription = null;


    }




    public void EditMessage()
    {
        //UIManager.Instance.OnBackButtonClicked();
        UIManager.Instance.ShowPanel(1);
        TableManager.Instance.selledTable = new List<MedicineData>(PrescriptionMessage.MedicineLsit);
        if (PrescriptionMessage.isPaid)
        {
            TableManager.Instance.returnMedicineList(PrescriptionMessage.MedicineLsit);//将处方单药物放回BankList
            PrescriptionMessage.MedicineLsit.Clear();
            PrescriptionMessage.isPaid = false;
        }
        ReturnPrescription();

        TableManager.Instance.Dropdown.value = 1;//切换到SellList
        TableManager.Instance.ChnageList();

    }



    public void RenturnCanvas()
    {

        UIManager.Instance.OnBackButtonClicked();
    }

    public void SetPrescriptMessage(PrescriptionMessage Message)
    {
        PrescriptionMessage = new PrescriptionMessage(Message);
    }



}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.AddressableAssets.Build.Layout.BuildLayout;

[System.Serializable]
public class PrescriptionMessage
{
    // Start is called before the first frame update
    [System.Serializable]
    public struct PatientMessage
    {
        public string Name;
        public string age;
        public string Date;
        public string Sex;
        public string Description;
        public string Location;
        public string number;
        public float T;//体温
        public float Weight;//体重
        public float P;//心率
        public float BP_b;
        public float BP_p;
        public float Blood_Sugar;
    }
    public PatientMessage Patient;
    public int ListId;
    public string PrescriptionId;
    public string Doctor;
    public string Check;
    public float SignCost;
    public float InjectionCost;
    public float TreatmentCost;
    public float MedicineCost;
    public float OriginalCost;
    public float OtherCost;
    public float SumCost;
    public float Profit;
    public List<MedicineData> MedicineLsit;
    //public bool isSave = false;
    public bool isPaid = false;


    public PrescriptionMessage(PrescriptionMessage Message)
    {
        Patient = Message.Patient;
        ListId = Message.ListId;
        Doctor= Message.Doctor;
        Check= Message.Check;
        PrescriptionId= Message.PrescriptionId;
        MedicineCost = Message.MedicineCost;
        OriginalCost= Message.OriginalCost;
        isPaid = Message.isPaid;
        SignCost= Message.SignCost;
        InjectionCost= Message.InjectionCost;
        TreatmentCost= Message.TreatmentCost;
        OtherCost=Message.OtherCost;
        SumCost = Message.SumCost;
        Profit= Message.Profit;
        MedicineLsit = Message.MedicineLsit != null ? new List<MedicineData>(Message.MedicineLsit) : new List<MedicineData>();

    }
    public PrescriptionMessage(int Index)
    {
        this.Patient.Name = "Name";
        this.Patient.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ListId = Index;

        // 获取当前的年月（yyyy-MM 格式）
        string currentMonth = DateTime.Now.ToString("yyyy-MM");

        // 获取所有已经存在的当前月的PrescriptionId（假设格式为 "yyyy-MM-xxx"）
        var existingIds = PrescriptManager.instance.PrescriptList
            .Where(pm => pm.PrescriptionId.StartsWith(currentMonth))  // 只考虑以当前月份开头的ID
            .Select(pm => pm.PrescriptionId.Substring(8)) // 提取出编号部分（即 "xxx"），格式从 "yyyy-MM-" 后开始
            .Select(id => int.TryParse(id, out var num) ? num : (int?)null) // 转换为整数
            .Where(num => num.HasValue) // 过滤掉无效值
            .OrderBy(num => num.Value) // 按照数字排序
            .ToList();

        // 寻找最小的缺失编号
        int newIdNumber = 1;
        foreach (var existingId in existingIds)
        {
            if (existingId != newIdNumber) // 如果有空缺编号
            {
                break; // 找到第一个空缺的编号
            }
            newIdNumber++;
        }

        // 创建新的 PrescriptionId，格式为 "yyyy-MM-xxx"（xxx为当前月的缺失编号）
        this.PrescriptionId = currentMonth + "-" + newIdNumber.ToString("D3");

    }
    public PrescriptionMessage() {
        this.Patient.Name = "Name";
        this.Patient.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    }


}

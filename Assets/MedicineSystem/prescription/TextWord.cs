using Aspose.Words;
using Aspose.Words.Tables;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class TextWord : MonoBehaviour
{
    public PrescriptionMessage Prescription;
    public string SavePath;

    // 保存处方的方法
    public void SavePrescription()
    {
        //Application.persistentDataPath
        string customPath = SavePath;
        Prescription = this.transform.GetComponent<prescription>()?.PrescriptionMessage;

        // 如果没有传入自定义路径，则使用默认的路径
        string filePath;
        if (string.IsNullOrEmpty(customPath))
        {
            filePath = Path.GetFullPath("Updated_Prescription_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx");
        }
        else
        {
            // 创建文件夹路径
            if (!Directory.Exists(customPath))
            {
                try
                {
                    Directory.CreateDirectory(customPath);
                    UnityEngine.Debug.Log($"文件夹 {customPath} 已创建。");
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"无法创建文件夹: {ex.Message}");
                    return;
                }
            }

            // 使用Prescription的名字和时间来生成文件名
            string fileName = $"{Prescription.Patient.Name}_{Prescription.Patient.Date.Replace(":", "-").Replace(" ", "_")}.docx";
            filePath = Path.Combine(customPath, fileName);

            // 如果文件已存在，覆盖文件
            if (File.Exists(filePath))
            {
                UnityEngine.Debug.Log($"文件已存在，正在覆盖: {filePath}");
            }
        }

        // 生成处方文档
        UnityEngine.Debug.Log("正在保存处方... 使用的路径: " + customPath);
        UnityEngine.Debug.Log("文件名生成: " + Path.GetFileName(filePath));
        GeneratePrescriptionDocument(filePath);
        UnityEngine.Debug.Log($"Word 文档已生成！路径: {filePath}");

        // 自动打开文件
        OpenFile(filePath);
    }
    public void GeneratePrescriptionDocument(string filePath)
    {
        try
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // 设置页面大小
            doc.FirstSection.PageSetup.PaperSize = PaperSize.Custom;
            doc.FirstSection.PageSetup.PageWidth = ConvertUtil.InchToPoint(5.5);
            doc.FirstSection.PageSetup.PageHeight = ConvertUtil.InchToPoint(8.5);
            doc.FirstSection.PageSetup.RightMargin = ConvertUtil.MillimeterToPoint(4);
            doc.FirstSection.PageSetup.LeftMargin = ConvertUtil.MillimeterToPoint(8);
            doc.FirstSection.PageSetup.TopMargin = 36;
            doc.FirstSection.PageSetup.BottomMargin = 36;

            // 设置标题
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Font.Size = 16;
            builder.Font.Name = "宋体"; // 中文标题用宋体
            builder.Writeln("多祝镇皇思扬村卫生站处方笺");
            builder.Font.Size = 10;
            builder.Writeln();

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Writeln($"No.{Prescription.PrescriptionId}");

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.Writeln($"就诊时间:{Prescription.Patient.Date}");
            builder.Writeln();

            builder.Font.Underline = Underline.Single;
            builder.Write($"姓名:{Prescription.Patient.Name}\t\t性别:{Prescription.Patient.Sex}\t\t年龄:{Prescription.Patient.age}");
            builder.Writeln();

            if (Prescription.Patient.T != 0)
                builder.Write($"体温:{Prescription.Patient.T}度\t");
            if (Prescription.Patient.P != 0)
                builder.Write($"心率:{Prescription.Patient.P}次/分\t");
            if (Prescription.Patient.BP_b != 0 || Prescription.Patient.BP_p != 0)
                builder.Write($"血压:{Prescription.Patient.BP_b}/{Prescription.Patient.BP_p}mmhg\t");
            if (Prescription.Patient.Blood_Sugar != 0)
                builder.Write($"血糖:{Prescription.Patient.Blood_Sugar}mmol/L");
            builder.Writeln();

            builder.Writeln($"临床诊断:{Prescription.Patient.Description}");
            builder.Writeln();

            builder.Writeln($"地址:{Prescription.Patient.Location}\t电话:{Prescription.Patient.number}");
            builder.Writeln();

            builder.Font.Underline = Underline.None;
            builder.Writeln("Rp.");
            builder.Writeln();

            // 设置等宽字体用于精确对齐
            builder.Font.Name = "Courier New";
            builder.Font.Size = 10;

            // 每列起始位置（字符位置）
            int col1Start = 0;    // 编号起始位置
            int col2Start = 1;   // 药名起始位置
            int col3Start = 20;   // 规格起始位置
            int col4Start = 35;   // 用法起始位置
            int col5Start = 23;
            // 药品部分：精确对齐
            for (int i = 0; i < Prescription.MedicineLsit.Count; i++)
            {
                var med = Prescription.MedicineLsit[i];

                // 第一行：编号 + 药品名 + 规格
                string line1 = "";
                line1 = line1.PadRight(col1Start) + (i + 1).ToString()+".";  // 编号部分
                line1 = line1 + med.MedicineMessage.Name;  // 药品名部分
                string tabs = GetTabs(line1,true);
                line1 += tabs;
                line1 = line1+ $"{med.MedicineMessage.dosage}{med.MedicineMessage.doseunit}*" +
                                                        $"{med.MedicineMessage.capacity}{med.MedicineMessage.minunit}/" +
                                                        $"{med.MedicineMessage.maxunit}";  // 规格部分
                builder.Writeln(line1);

                // 第二行：用法、每次、每日等
                string line2 = "";
                string usewayText = $"用法:{med.Useway.ToString().Replace("_", "/")}";
                tabs= GetTabs(usewayText,false);
                line2 = usewayText + tabs;
                line2 += $"每次:{med.eatdose}{med.MedicineMessage.minunit}\t";
                line2 += $"每日:{med.Times}次 {med.Day}天 {med.addnumber}{med.MedicineMessage.minunit}";

                builder.Writeln(line2);
                builder.Writeln(); 
            }

            // 恢复字体
            builder.Font.Name = "宋体";
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.Writeln($"医师:{Prescription.Doctor}\t挂号费{Prescription.SignCost}\t注射费{Prescription.InjectionCost}" +
                            $"\t治疗费{Prescription.TreatmentCost}\t材料费{Prescription.Profit}");
            builder.Writeln();

            builder.Writeln($"审核:{Prescription.Check}\t药品费{Prescription.MedicineCost}\t基本费{Prescription.OriginalCost}" +
                            $"\t其他{Prescription.OtherCost}\t\t总计:{Prescription.SumCost}");

            doc.Save(filePath);
            UnityEngine.Debug.Log($"文档已成功保存至 {filePath}");
        }
        catch (NotSupportedException ex)
        {
            UnityEngine.Debug.LogError($"编码错误: {ex.Message}");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"生成处方文档时发生异常: {ex.Message}");
        }
    }



    public string GetTabs(string value,bool isChinese)
    {
        int length = value.Length;
        int tabCount;
        // 假设一个 \t 大概占 8 字符宽度（在等宽字体下）
        if (isChinese)
        {
             tabCount = (32 - length*2) / 8;
        }
        else
        {
             tabCount = (32 - length) / 8;
        }

        tabCount = Math.Max(tabCount, 1); // 至少 1 个 \t
        string tabs = new string('\t', tabCount);
        return tabs;
    }




    //public void GeneratePrescriptionDocument(string filePath)
    //{
    //    try
    //    {
    //        Document doc = new Document();
    //        DocumentBuilder builder = new DocumentBuilder(doc);

    //        // 设置标题
    //        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
    //        builder.Font.Size = 22;
    //        builder.Writeln("多祝镇皇思扬村卫生站处方笺");
    //        builder.Font.Size = 12;
    //        builder.Writeln();

    //        builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
    //        builder.Writeln($"No.{Prescription.PrescriptionId}");
    //        // 左对齐的就诊时间
    //        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
    //        builder.Writeln($"就诊时间:{Prescription.Patient.Date}");
    //        builder.Writeln();

    //        // 患者信息
    //        builder.Font.Underline = Underline.Single;
    //        builder.Write($"姓名:{Prescription.Patient.Name}\t\t性别:{Prescription.Patient.Sex}\t\t年龄:{Prescription.Patient.age}");
    //        builder.Writeln();

    //        // PatientMessage
    //        if (Prescription.Patient.T != 0)
    //            builder.Write($"体温:{Prescription.Patient.T}度\t");
    //        if (Prescription.Patient.P != 0)
    //            builder.Write($"心率:{Prescription.Patient.P}次/分\t");
    //        if (Prescription.Patient.BP_b != 0 || Prescription.Patient.BP_p != 0)
    //            builder.Write($"血压:{Prescription.Patient.BP_b}/{Prescription.Patient.BP_p}mmhg\t");
    //        if (Prescription.Patient.Blood_Sugar != 0)
    //            builder.Write($"血糖:{Prescription.Patient.Blood_Sugar}mmol/L");
    //        builder.Writeln();

    //        // 临床诊断
    //        builder.Writeln($"临床诊断:{Prescription.Patient.Description}");
    //        builder.Writeln();

    //        // 地址和电话
    //        builder.Writeln($"地址:{Prescription.Patient.Location}\t电话:{Prescription.Patient.number}");
    //        builder.Writeln();
    //        builder.Font.Underline = Underline.None;
    //        // 处方部分
    //        builder.Writeln("Rp.");
    //        builder.Writeln();

    //        // 处方表格
    //        Table prescriptionTable = builder.StartTable();

    //        // 第一行
    //        for (int i = 0; i < Prescription.MedicineLsit.Count; i++)
    //        {

    //            builder.InsertCell(); builder.Write($"{i + 1}");
    //            builder.InsertCell(); builder.Write($"{Prescription.MedicineLsit[i].MedicineMessage.Name}");
    //            builder.InsertCell(); builder.Write($"{Prescription.MedicineLsit[i].MedicineMessage.dosage}{Prescription.MedicineLsit[i].MedicineMessage.doseunit}" +
    //                $"*{Prescription.MedicineLsit[i].MedicineMessage.capacity}{Prescription.MedicineLsit[i].MedicineMessage.minunit}/{Prescription.MedicineLsit[i].MedicineMessage.maxunit}");
    //            builder.EndRow();


    //            // 用法
    //            builder.InsertCell();
    //            builder.Write($"用法:{Prescription.MedicineLsit[i].Useway.ToString().Replace("_", "/")}");
    //            builder.InsertCell();
    //            builder.Write($"每次:{Prescription.MedicineLsit[i].eatdose}{Prescription.MedicineLsit[i].MedicineMessage.minunit}");
    //            builder.InsertCell();
    //            builder.Write($"每日{Prescription.MedicineLsit[i].Times}次\t{Prescription.MedicineLsit[i].Day}天\t{Prescription.MedicineLsit[i].addnumber}{Prescription.MedicineLsit[i].MedicineMessage.minunit}");
    //            builder.EndRow();

    //        }

    //        builder.EndTable();
    //        builder.Writeln();
    //        builder.Writeln();

    //        // 医师签名和金额 (底部对齐)
    //        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
    //        builder.Writeln($"医师:{Prescription.Doctor}\t\t挂号费{Prescription.SignCost}\t注射费{Prescription.InjectionCost}\t治疗费{Prescription.TreatmentCost}\t材料费{Prescription.Profit}");
    //        builder.Writeln();
    //        builder.Writeln($"审核:{Prescription.Check}\t\t药品费{Prescription.MedicineCost}\t基本费{Prescription.OriginalCost}\t其他{Prescription.OtherCost}\t总计:{Prescription.SumCost}");
    //        //builder.Writeln($"挂号费{Prescription.SignCost}\t注射费{Prescription.InjectionCost}\t治疗费{Prescription.TreatmentCost}\t" +
    //        //    $"药物费{Prescription.MedicineCost}\t总金额:{Prescription.MedicineCost}");
    //        //builder.Writeln();
    //        //builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
    //        //builder.Writeln($"总计:{Prescription.SumCost}");
    //        // 保存文档
    //        doc.Save(filePath);

    //        UnityEngine.Debug.Log($"文档已成功保存至 {filePath}");
    //    }
    //    catch (NotSupportedException ex)
    //    {
    //        UnityEngine.Debug.LogError($"编码错误: {ex.Message}");
    //    }
    //    catch (Exception ex)
    //    {
    //        UnityEngine.Debug.LogError($"生成处方文档时发生异常: {ex.Message}");
    //    }
    //}

    // 打开文件的方法
    public void OpenFile(string filePath)
    {
        try
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("无法打开文件: " + ex.Message);
        }
    }
}

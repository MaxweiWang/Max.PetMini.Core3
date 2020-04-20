using Max.PetMini.DAL.Entities;
using Max.PetMini.DAL.Models;
using Max.PetMini.DAL.Repositories;
using Max.PetMini.Extension.Exceptions;
using Max.PetMini.Extension.Models;
using NAutowired.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Max.PetMini.BLL.Services
{
    [Service]
    public class PoMasterService : Service
    {
       
        [Autowired]
        private readonly ExpressRepository expressRepository;

        //private void Check(DeliveryConfirmModel model, out PoMaster poMaster, out ErpPoMasterExt epme)
        //{
        //    poMaster = new PoMaster();
        //    epme = new ErpPoMasterExt();
        //    if (model.PoId <= 0)
        //        throw new BusinessException("参数无效，请尝试刷新页面");

        //    poMaster = poMasterRepository.GetByPoId(model.PoId);

        //    if (poMaster == null)
        //        throw new BusinessException("采购单无效，请尝试刷新页面");

        //    epme = erpPoMasterExtRepository.GetBySysNo(model.PoId);

        //    if (epme != null && epme.VendorDeliveryStatus == ErpPoMasterExt.VendorDeliveryStatusStruct.Yes)
        //        throw new BusinessException("该订单已经进行过发货确认");
        //}

        
        ///// <summary>
        ///// 自配发货确认
        ///// </summary>
        ///// <param name="model">发货确认model</param>
        ///// <returns></returns>
        //public void ConfirmZiPeiSave(DeliveryConfirmModel model)
        //{
        //    PoMaster poMaster = new PoMaster();
        //    ErpPoMasterExt epme = new ErpPoMasterExt();
        //    Check(model, out poMaster, out epme);

        //    var doMaster = doMasterRepository.GetDoMasterByPoReferId(poMaster.ReferId);

        //    if (doMaster != null && doMaster.IsAttachDeliveryBill != DoMaster.StructIsAttachDeliveryBill.None && doMaster.DownLoadDeliveryBillStatus != DoMaster.StructDownLoadDeliveryBillStatus.Done)
        //    {
        //        throw new BusinessException("当前订单发货需附带客户发货单，请下载后操作！");
        //    }

        //    //出库单
        //    string doId = string.Empty;
        //    int doMasterId = 0;
        //    SetDoIdAndDoCode(poMaster.ReferId, out doId, out doMasterId);

        //    //发送包裹记录（快递单）
        //    List<VendorSendParcelLog> parcelLog = ConvertZPVendorSendParcelLog(model);

        //    //将自配转为快递方式
        //    List<VendorSendParcelLog> parcelLogs = vendorSendParcelLogRepository.ConvertVendorSendParcelLog(parcelLog, doMasterId, doId, model.PoId, model.PoCode, true, model.CurrentUserId);

        //    try
        //    {
        //        SendParcelLogSaveSave(1, epme, model, parcelLogs, poMaster);

        //        if (doMaster != null && doMaster.ReferType == DoMaster.ReferTypeStruct.SO && doMaster != null && !doWmsLogRepository.DOLogExist(doMaster.SysNo, 299))
        //        {
        //            int soMasterSysno = 0;
        //            string soMasterId = string.Empty;
        //            SoMaster erpSoMaster = soMasterRepository.GetSoByPoSysNo(poMaster, true).FirstOrDefault();

        //            if (erpSoMaster != null)
        //            {
        //                soMasterSysno = erpSoMaster.SysNo;
        //                soMasterId = erpSoMaster.SOID;
        //            }
        //            doWmsLogRepository.DoWmsLogSave(soMasterSysno, soMasterId, doMasterId, doId, string.Format("您的包裹单{0}已出库", doId), model.CurrentUserId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Common.Log.AliyunLogService.Default.Error("供应商系统自配发货确认失败", ex.ToString(), "采购单编号: " + model.PoId + " ");
        //        throw new BusinessException("发货失败");
        //    }
        //}

    }
}

alter table SAJET.SYS_PROGRAM_NAME modify (PROGRAM_VN VARCHAR2(120));

Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P01 Thiết lập dữ liệu cơ bản'  Where Program = 'Data Center';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P02 Quản lý lệnh sản xuất'  Where Program = 'W/O Manager';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P03 Quản lý mã vạch'  Where Program = 'Barcode Center';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P04 TGS'  Where Program = 'TGSSetup';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P05 Sửa chữa trực tuyến'  Where Program = 'Repair';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P07 Kiểm tra chất lượng'  Where Program = 'Quality Control';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P08 Đóng gói'  Where Program = 'Packing';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P10 Tra cứu & Báo cáo'  Where Program = 'Query';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P11 Kiểm thử sản phẩm qua trạm'  Where Program = 'Product Test Transfer';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P15 IQC'  Where Program = 'IQC';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P19 Lắp ráp'  Where Program = 'Assembly';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P20 DIP'  Where Program = 'DIP';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P21 Quản lý vật tư phụ SMT'  Where Program = 'SMT Auxiliary Material';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P23 Hệ thống chống lỗi vật tư SMT'  Where Program = 'SMT';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P24 Module cảnh báo'  Where Program = 'Alarm';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P25 Quản lý đồ gá'  Where Program = 'ToolingManager';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P27 Quản lý WIP hàng loạt'  Where Program = 'WIP';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P28 Quản lý thời gian lao động'  Where Program = 'Work Hour';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P30 Xuất hàng'  Where Program = 'Shipping';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P41 Kệ vật liệu điện tử'  Where Program = 'Pick To Light';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P70 Quản lý kho WMS'  Where Program = 'WMS Inventory';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P200 SMT PDA'  Where Program = 'SMT PDA';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'P999 ICMES'  Where Program = 'ICMES';
Update SAJET.SYS_PROGRAM_NAME SET PROGRAM_VN = 'Quản lý phương tiện'  Where Program = 'Carrier Manager';

commit;


% data2=xlsread('data_3err_RULDRM'); % 错误数据
Acc_x_normal=data1101(1:1000,2);
Acc_y_normal=data1101(1:1000,3);
Acc_z_normal=data1101(1:1000,4);
% Acc_x_error=data2(97:296,1);  % 只需要改区间即可
% Acc_y_error=data2(97:296,2);
% Acc_z_error=data2(97:296,3);
t=1:1000; % 选取200点
%% 正常数据与故障数据分图对比
figure
set(gcf,'outerposition',get(0,'screensize'));
subplot(3,2,1)
plot(t,Acc_x_normal);
title('Accx normal')
xlabel('t');
ylabel('Accx');
subplot(3,2,3)
plot(t,Acc_y_normal);
title('Accy normal')
xlabel('t');
ylabel('Accy');
subplot(3,2,5)
plot(t,Acc_z_normal);
title('Accz normal')
xlabel('t');
ylabel('Accz');

% subplot(3,2,2)
% plot(t,Acc_x_error);
% title('Accx error')
% xlabel('t');
% ylabel('Accx');
% subplot(3,2,4)
% plot(t,Acc_y_error);
% title('Accy error')
% xlabel('t');
% ylabel('Accy');
% subplot(3,2,6)
% plot(t,Acc_z_error);
% title('Accz error')
% xlabel('t');
% ylabel('Accz');

%% 正常数据和故障数据同图对比
% figure 
% plot(t,Acc_x_normal);
% hold on 
% plot(t,Acc_x_error);
% title('Accx')
% legend('normal','error');
% figure 
% plot(t,Acc_y_normal);
% hold on 
% plot(t,Acc_y_error);
% title('Accy')
% legend('normal','error');
% figure 
% plot(t,Acc_z_normal);
% hold on 
% plot(t,Acc_z_error);
% title('Accz')
% legend('normal','error');
%% 误差图
% figure 
% error_x=Acc_x_normal-Acc_x_error;
% error_y=Acc_y_normal-Acc_y_error;
% error_z=Acc_z_normal-Acc_z_error;
% subplot(3,1,1)
% plot(t,error_x)
% title('Diff X');
% subplot(3,1,2)
% plot(t,error_y)
% title('Diff Y');
% subplot(3,1,3)
% plot(t,error_z)
% title('Diff Z');
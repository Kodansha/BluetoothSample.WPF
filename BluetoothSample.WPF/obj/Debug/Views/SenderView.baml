   M S B A M L   `   `   `  ����  BluetoothSample.WPF&     BluetoothSample.Views.SenderView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � DataContext  5      �� *SenderViewModel+   Locator �� Source# [  .��5      �� $��20q�6    �� Orientation$ Vertical=�6   .+�5   
   �� I am the Sender.6&   �� 5   
   $��0,20,0,0q�6   Select one device:6:   �� 5   
   $	��200��6   $	��100��5      $	��200��5      $	��100��5      $��0,20,0,0q�5      $
��Left=�5      ��5      �� *	Devices+ �� SelectedItem 5      �� *SelectDevice+�� 5   
   $��80��6   $��5,5,0,0q�5      $
��Left=�5      $��Connect��5      ��5      �� *ConnectDevice+5      �� 5   
   $��0,5,0,0q�6   $
��Wrap=�6%   
Status :�� 5      �6   �� *DeviceStatus+5      �� 5   
   $��0,10,0,0q�6   $��Write the data to send:��6&   �� 5   
   $��	0,10,10,0q�6   ��6%   �� *Data+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5    
   -   5        $��80��5       $��	0,10,20,0q�5!      $��Right=�5"      $
��Send��5$      ��5#      �� *SendCommand+5$       �� 5%   
   $��0,5,0,0q�6   $
��Wrap=�6%   	Result:�� 5&      �6   �� *ResultValue+5'      5(      5)      
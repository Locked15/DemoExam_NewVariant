   M S B A M L   `   `   `  ���� J  F41 размер, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:_41_размер.UserControls_41_размер.UserControls  1    +_41_размер.UserControls.OrderListItem   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     =local+clr-namespace:_41_размер.UserControls     $	��150��$	��800��
�������
���   �$��1q�  � ContextMenu  �� .@�m� -   $��MainContextAction��$��[Init_This_In_Code]��m� -   $��SecondContextAction��$��Удалить заказ��.��� $	��1.5q� � RowDefinitions �� �� �� ��  � ColumnDefinitions �� $
��200* ��� $
��130* ��� $	��40* ��� $
��250* ��� $
��180* �.+��� $��0��$��0��$��4��
���   �$	��1.5q��� $��0��$��0��$ ��Состав заказа:��$��20(��� -   $��OrderProductsList��$��1��$��0��$��3��$
��Wrap=� �� TextAlignment$
 Left=�$	��Top=�$��5q�����  �� Path$  AllProductsInOrderAsString��� $��0��$��1��$��2��$��24(����� $ OrderID��� $��0��$��2��$��2��$��|��$��28(��� -   $��OrderConsumerInfo��$��0��$��3��$��2��$��20(����� $ User.FullName��� $��2��$��1��$��2��$��24(����� $ OrderBeginDate� �� StringFormat$ 
dd.MM.yyyy���� $��2��$��2��$��2��$��|��$��26(��� $��2��$��3��$��2��$��24(����� $ OrderDeliveryDate�$ 
dd.MM.yyyy���� $��0��$��4��$��Стоимость:��$��24(��� $��1��$��4��$��24(����� $ 	FinalCost�$
 0.00���� $��2��$��4��$��Скидка (%):��$��24(��� $��3��$��4��$��24(����� $ FinalDiscount�
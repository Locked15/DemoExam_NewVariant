   M S B A M L   `   `   `  ���� J  F41 размер, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:_41_размер.UserControls_41_размер.UserControls  3    -_41_размер.UserControls.ProductListItem   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     =local+clr-namespace:_41_размер.UserControls     -   $	��150��$	��800��
���   �$	��1.5q�
�������  � ContextMenu  �� .@�m� -   $��	AddToCart��$(��"Добавить в корзину��m� -   $��RemoveFromCart��$(��"Удалить из корзины��.��� $	��7.5q� � RowDefinitions �� $	��30* ��� $	��90* ��� $	��30* ��� $	��30* � � ColumnDefinitions �� $
��200* ��� $	��30* ��� $
��320* ��� $	��30* ��� $
��190* �.+��� $��0��$��0��$��4��
���   �$	��1.5q��� -   $��ProductImage��$��0��$��0��$��4��$
��Fill=� �� StretchDirection$
 Both=��� $��0��$��2��$��4��
���   �$	��0.5q��� $��0��$��2��$��16(� �� TextAlignment$
 Left=�����  �� Path$ ProductName��� $��1��$��2��$��14(�$
 Left=�$	��Top=�$
��Wrap=�$	��2.5q����� $ ProductDescription��� -   $��ProductManufacturerInfo��$��2��$��2��$��16(�$
 Left=�$	��2.5q��� $��3��$��2�� �� Orientation$ 
Horizontal=�.+��� -   $��ProductOriginPrice��$��16(�$
 Left=����� $ ProductCost� �� StringFormat$
 0.00���� -   $��ProductDiscountPrice��$y�Hidden=�$��16(�$
 Left=�$��10, 0, 0, 0q����� $ 	FinalCost�$
 0.00���� $��0��$��4��$��4��
���   �$	��1.5q��� $��0��$��4��$��18(�$��Скидка:���� -   $��DiscountSize��$��0��$��4��$��4��$��48(�
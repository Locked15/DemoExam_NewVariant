   M S B A M L   `   `   `  ����J  F41 размер, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:_41_размер.Windows_41_размер.Windows  5   	   -    '_41_размер.Windows.OrderFormation   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:_41_размер.Windows     5   	   -   5          � Title$9  3Настройка заказа — 41 размер��5   	   $	��600��5	   	   $	��800��6   $	��575��5
   	   $	��775��6   .��5      � $	��2.5q�6    � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $	��40* �6   �� 5      $	��40* �6   �� 5      $	��40* �6   �� 5      $
��370* �6   �� 5      $	��30* �6   �� 5      $	��10* �6   �� 5      $	��60* �6   .+�5%   
   �� -   5        $��UserInfoDesc��5%      $��0��6(   $��0��65   $��Пользователь:��6E   $��20(�6Z   �� 5&   
   -   5        $��UserInfo��5&      $��0��6$   $��1��61   $��20(�6r   ��6A   ��  �� Path$  CurrentOrder.User.FullName�6r   �� 5(   
   $��0��6   $��2��6!   $ ��Код получения:��61   $��20(�6G   �� 5)   
   $��0��6   $��3��6!   $��20(�6]   ��61   �� $ CurrentOrder.TakeCode�6]   �� 5+   
   $��1��6   $��0��6!   $$��Место получения:��61   $��20(�6I   �� 5,   
   -   5        $��PickerPoint��5,      $��1��6&   $��1��63    �� DisplayMemberPath$ Point��6C   6]   �� 5.   
   $��1��6   $��2��6!   $"��Дата получения:��61   $��20(�6H   (  "System.Windows.Controls.DatePicker  5/   
   -   5        $��SelectDeliveryDate��5/      $��1��6/   $��3��6<   $��20(�6�      SelectedDate 6L   �� $$ CurrentOrder.OrderDeliveryDate� �� StringFormat$ 
dd.MM.yyyy��6�   �� 51   
   $��2��6   $��0��6!   $��Стоимость:��61   $��20(�6C   �� 52   
   -   5        $��OrderFinalCost��52      $��2��6*   $��1��67   $��20(�6G   �� 54   
   $��2��6   $��2��6!   $$��Итоговая скидка:��61   $��20(�6I   �� 55   
   -   5        $��OrderFinalDiscount��55      $��2��6.   $��3��6;   $��20(�6K   �� 58   
   -   5        $��ProductsInOrder_ListView��58      $��3��63   $��0��6@   $��4��6P    �� SelectionMode$ Single=�6d   59      �� 5=   
   $��4��6   $��0��6!   $��2��61   $(��"Количество товара:��6E   $��20(�6_   �� 5>   
   -	   5        $��CurrentProductCount��5>      $��4��6-   $��2��6:   $��2��6J   $��20(�6^   5?      �� 5A   
   -
   5        $��GeneratePdfDocumentation��5A      $��6��61   $��0��6>   $��2��6N   $1��+Сформировать накладную��6b   $��20(�6�   5B      �� 5C   
   -   5        $��SaveOrderInDb��5C      $��6��6&   $��2��63   $��2��6C   $#��Сохранить заказ��6W   $��20(�6q   5D      5F      5G      
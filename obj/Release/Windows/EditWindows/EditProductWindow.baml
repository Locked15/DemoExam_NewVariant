   M S B A M L   `   `   `  ���� J  F41 размер, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:_41_размер.Windows_41_размер.Windows  0    *_41_размер.Windows.EditProductWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:_41_размер.Windows     -     � Title$9  3Настройка товара — 41 размер��$	��650��$	��400��$	��625��$	��375��.��� $	��1.5q� � RowDefinitions �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $
��7.5* ��� $	��35* ��� $
��7.5* ��� $	��35* ��� $
��7.5* ��� $��5* ��� $	��15* � � ColumnDefinitions �� �� .+��� $��0��$��0��$��Артикул:��$��18(� �� TextAlignment$
 Left=��� -   $��EditArticleNumber��$��0��$��1��$��18(�����  �� Path$ ProductArticleNumber��� $��1��$��0��$��Название:��$��18(�$
 Left=��� -   $��EditProductName��$��1��$��1��$��18(����� $ ProductName��� $��2��$��0��$(��"Единица измерения:��$��18(�$
 Left=��� -   $��EditProductUnit��$��2��$��1��$��18(����� $ ProductUnit��� $��3��$��0��$��Макс. скидка:��$��18(�$
 Left=��� -   $��EditMaxDiscount��$��3��$��1��$��18(����� $ ProductMaxDiscount��� $��4��$��0��$"��Текущая скидка:��$��18(�$
 Left=��� -   $��EditProductDiscountAmount��$��4��$��1��$��18(����� $ ProductDiscountAmount��� $��5��$��0��$(��"Базовая стоимость:��$��18(�$
 Left=��� -   $��EditProductCost��$��5��$��1��$��18(����� $ ProductCost� �� StringFormat$
 0.00���� $��6��$��0��$$��Кол-во на складе:��$��18(�$
 Left=��� -   $��EditQuantityInStock��$��6��$��1��$��18(����� $ ProductQuantityInStock��� $��7��$��0��$&�� Категория товара:��$��18(�$
 Left=��� -	   $��EditProductCategory��$��7��$��1��$��18(����� $ ProductCategory��� $��8��$��0��$!��Производитель:��$��18(�$
 Left=��� -
   $��EditManufacturer��$��8��$��1��$��18(� �� DisplayMemberPath$ CompanyName���� $��9��$��0��$��Поставщик:��$��18(�$
 Left=��� -   $��EditDeliver��$��9��$��1��$��18(�$ CompanyName���� $��10��$��0��$��2��$��Описание��$��16(��� -   $��EditDescription��$��11��$��0��$��2�� �� TextWrapping$
 Wrap=� �� TextAlignment$
 Left=�$	��Top=����� $ ProductDescription��� $��12��$��0��$��2��$��Изображение��$��16(��� -   $��ShowProductPhoto��$��13��$��0��$��2��$
��Fill=�	 �� EdgeMode$	 Aliased=����� $ ProductFullPhotoPath��� -   $��EditProductPhoto��$��14��$��0��$��2��$��Изменить��$��16(��� -   $��DeleteProduct��$��16��$��0��$��Удалить товар��$��18(�$	��2.5q��� -   $��SaveProduct��$��16��$��1��$��Сохранить��$��18(�$	��2.5q�
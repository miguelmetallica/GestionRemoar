
INSERT [ParamPresupuestosEstados] ([Id], [Codigo], [Descripcion], [Estado]) VALUES (N'095ce521-baf5-4090-9bf8-f435fb6ce294', N'002', N'APROBADO (CLIENTE)', 1)
GO
INSERT [ParamPresupuestosEstados] ([Id], [Codigo], [Descripcion], [Estado]) VALUES (N'98c95f66-ee4f-41bc-85c2-581e794c442c', N'001', N'PENDIENTE (CLIENTE)', 1)
GO
INSERT [ParamPresupuestosEstados] ([Id], [Codigo], [Descripcion], [Estado]) VALUES (N'ba693588-9281-4b2d-8e38-e8f54eb0169d', N'003', N'RECHAZADO (CLIENTE)', 1)
GO
INSERT [ParamPresupuestosDescuentos] ([Id], [Descripcion], [Porcentaje], [Estado]) VALUES (N'69c36400-1776-40af-a4df-32ea0942de00', N'DESCUENTO DE 5% ', CAST(5.00 AS Numeric(18, 2)), 1)
GO
INSERT [ParamPresupuestosDescuentos] ([Id], [Descripcion], [Porcentaje], [Estado]) VALUES (N'96f83d71-ea73-4cb1-9de2-df3bcf92acc7', N'DESCUENTO DE 15% ', CAST(15.00 AS Numeric(18, 2)), 0)
GO
INSERT [ParamPresupuestosDescuentos] ([Id], [Descripcion], [Porcentaje], [Estado]) VALUES (N'dde613d2-7dae-489a-93bb-fb71dd7fc7ec', N'DESCUENTO DE 3% ', CAST(3.00 AS Numeric(18, 2)), 1)
GO
INSERT [ParamPresupuestosDescuentos] ([Id], [Descripcion], [Porcentaje], [Estado]) VALUES (N'e9a5a6db-8a05-40d3-ac8d-83ef26c7cf74', N'DESCUENTO DE 7% ', CAST(7.00 AS Numeric(18, 2)), 1)
GO
INSERT [ParamProvincias] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'0379de85-75c6-444d-89f6-9a67400c85a2', N'14', N'TUCUMAN', 1, 1, NULL, NULL)
GO
INSERT [ParamTiposDocumentos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'05acf025-5a87-4602-b95d-975a8f68a462', N'96', N'D.N.I.', 1, 1, NULL, NULL)
GO
INSERT [ParamTiposDocumentos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'a5f05a85-88ec-4622-bf20-2389e8e16ff1', N'90', N'LC', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposDocumentos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'b7ab49a7-fe3a-4027-85cc-0fbde8adc75f', N'89', N'LE', 0, 1, NULL, NULL)
GO
INSERT [Clientes] ([Id], [Codigo], [Apellido], [Nombre], [RazonSocial], [TipoDocumentoId], [NroDocumento], [CuilCuit], [esPersonaJuridica], [FechaNacimiento], [ProvinciaId], [Localidad], [CodigoPostal], [Calle], [CalleNro], [PisoDpto], [OtrasReferencias], [Telefono], [Celular], [Email], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'33cec5f6-4355-4267-8d2b-3c9cee1fbe23', N'00100000003', N'PRUEBA 2', N'PRUEBA', N'PRUEBA 2 PRUEBA', N'05acf025-5a87-4602-b95d-975a8f68a462', N'12345678', N'12345678901', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'11111111111111', NULL, 1, CAST(N'2020-08-22T08:41:51.780' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Clientes] ([Id], [Codigo], [Apellido], [Nombre], [RazonSocial], [TipoDocumentoId], [NroDocumento], [CuilCuit], [esPersonaJuridica], [FechaNacimiento], [ProvinciaId], [Localidad], [CodigoPostal], [Calle], [CalleNro], [PisoDpto], [OtrasReferencias], [Telefono], [Celular], [Email], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'33e35524-b18c-4c2d-aa9b-742c18d1dbf5', N'00100000002', N'PRUEBA 1', N'PRUEBA', N'PRUEBA 1 PRUEBA', N'05acf025-5a87-4602-b95d-975a8f68a462', N'11111111', NULL, 0, NULL, N'0379de85-75c6-444d-89f6-9a67400c85a2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'381020964', NULL, 1, CAST(N'2020-08-19T17:56:25.950' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Clientes] ([Id], [Codigo], [Apellido], [Nombre], [RazonSocial], [TipoDocumentoId], [NroDocumento], [CuilCuit], [esPersonaJuridica], [FechaNacimiento], [ProvinciaId], [Localidad], [CodigoPostal], [Calle], [CalleNro], [PisoDpto], [OtrasReferencias], [Telefono], [Celular], [Email], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'00100000001', N'ELIAS', N'MIGUEL ANGEL', N'ELIAS MIGUEL ANGEL', N'05acf025-5a87-4602-b95d-975a8f68a462', N'29879250', N'20298792509', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'32131321', NULL, 1, CAST(N'2020-08-19T14:17:17.917' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Clientes] ([Id], [Codigo], [Apellido], [Nombre], [RazonSocial], [TipoDocumentoId], [NroDocumento], [CuilCuit], [esPersonaJuridica], [FechaNacimiento], [ProvinciaId], [Localidad], [CodigoPostal], [Calle], [CalleNro], [PisoDpto], [OtrasReferencias], [Telefono], [Celular], [Email], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'e92f193c-ce1d-4762-be6a-86fbece5b666', N'00100000004', N'PRUEBA 3', N'PRUEBA', N'PRUEBA 3 PRUEBA', N'05acf025-5a87-4602-b95d-975a8f68a462', N'12378912', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'3815020964', NULL, 1, CAST(N'2020-08-28T12:22:23.013' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'41666916-076a-4316-b664-6cdf1e32b152', N'00100000003', CAST(N'2020-08-16T17:56:26.023' AS DateTime), CAST(N'2020-08-19T17:56:26.023' AS DateTime), N'33e35524-b18c-4c2d-aa9b-742c18d1dbf5', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-19T17:56:26.023' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'4c114592-8475-49fb-afcd-5a6ab82cca61', N'00100000011', CAST(N'2020-08-22T08:30:34.103' AS DateTime), CAST(N'2020-08-25T08:30:34.103' AS DateTime), N'33cec5f6-4355-4267-8d2b-3c9cee1fbe23', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-22T08:30:34.103' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'4d70f28f-e568-4248-936d-71b4db2fd9e1', N'00100000010', CAST(N'2020-08-21T17:25:54.573' AS DateTime), CAST(N'2020-08-24T17:25:54.573' AS DateTime), N'33e35524-b18c-4c2d-aa9b-742c18d1dbf5', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-21T17:25:54.573' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'4de1bfdf-93d6-418e-a4d5-623fef5561ce', N'00100000001', CAST(N'2020-08-16T14:17:46.573' AS DateTime), CAST(N'2020-08-19T14:17:46.573' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-19T14:17:46.573' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'533face2-6363-4bde-9f4f-6ed097b6cebd', N'00100000007', CAST(N'2020-08-17T19:04:38.510' AS DateTime), CAST(N'2020-08-20T19:04:38.510' AS DateTime), N'33e35524-b18c-4c2d-aa9b-742c18d1dbf5', N'98c95f66-ee4f-41bc-85c2-581e794c442c', NULL, NULL, 1, CAST(N'2020-08-20T19:04:38.510' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'60d45af4-f628-452d-baf1-fd8baf370dd0', N'00100000045', CAST(N'2020-08-25T16:08:34.870' AS DateTime), CAST(N'2020-08-28T16:08:34.870' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-25T16:08:34.870' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'63a35468-74f2-4175-982d-3fae31e2b436', N'00100000004', CAST(N'2020-08-17T19:03:35.110' AS DateTime), CAST(N'2020-08-20T19:03:35.110' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-20T19:03:35.110' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'00100000002', CAST(N'2020-08-16T14:36:48.013' AS DateTime), CAST(N'2020-08-19T14:36:48.013' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-19T14:36:48.013' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'71e38b01-f7bc-41d6-b294-8300a3d52f43', N'00100000008', CAST(N'2020-08-17T19:05:45.553' AS DateTime), CAST(N'2020-08-20T19:05:45.553' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'98c95f66-ee4f-41bc-85c2-581e794c442c', NULL, NULL, 1, CAST(N'2020-08-20T19:05:45.553' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'87c93f7a-5857-4fe8-8cf3-4aacf2d5e37a', N'00100000046', CAST(N'2020-08-25T16:10:15.740' AS DateTime), CAST(N'2020-08-28T16:10:15.740' AS DateTime), N'33e35524-b18c-4c2d-aa9b-742c18d1dbf5', N'98c95f66-ee4f-41bc-85c2-581e794c442c', NULL, NULL, 1, CAST(N'2020-08-25T16:10:15.740' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'976CFAAC-D2EF-40E4-B8E9-3C0358E6AF1E', N'00100000043', CAST(N'2020-08-24T08:35:43.580' AS DateTime), CAST(N'2020-08-27T08:35:43.580' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-24T08:35:43.580' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'a2df9d8b-e32a-4066-b55e-10ca40658f7e', N'00100000005', CAST(N'2020-08-17T19:03:55.403' AS DateTime), CAST(N'2020-08-20T19:03:55.403' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-20T19:03:55.403' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'aaaef321-686b-4fee-b017-3efc36af8c26', N'00100000044', CAST(N'2020-08-24T08:37:16.367' AS DateTime), CAST(N'2020-08-27T08:37:16.367' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'ba693588-9281-4b2d-8e38-e8f54eb0169d', NULL, NULL, 1, CAST(N'2020-08-24T08:37:16.367' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'c7ed29d7-8e95-40d7-b407-efec8d01a564', N'00100000009', CAST(N'2020-08-21T17:18:26.897' AS DateTime), CAST(N'2020-08-24T17:18:26.897' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'095ce521-baf5-4090-9bf8-f435fb6ce294', NULL, NULL, 1, CAST(N'2020-08-21T17:18:26.897' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'cd1fb442-02a3-48f2-904d-36103cc59513', N'00100000048', CAST(N'2020-08-28T12:22:23.027' AS DateTime), CAST(N'2020-08-31T12:22:23.027' AS DateTime), N'e92f193c-ce1d-4762-be6a-86fbece5b666', N'095ce521-baf5-4090-9bf8-f435fb6ce294', N'69c36400-1776-40af-a4df-32ea0942de00', CAST(5.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-28T12:22:23.027' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'de1caa75-58a4-49bb-afe9-d61b6d819826', N'00100000047', CAST(N'2020-08-28T12:20:49.457' AS DateTime), CAST(N'2020-08-31T12:20:49.457' AS DateTime), N'33cec5f6-4355-4267-8d2b-3c9cee1fbe23', N'98c95f66-ee4f-41bc-85c2-581e794c442c', NULL, CAST(0.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-28T12:20:49.457' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Presupuestos] ([Id], [Codigo], [Fecha], [FechaVencimiento], [ClienteId], [EstadoId], [DescuentoId], [DescuentoPorcentaje], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'eef7b402-107b-4d43-a164-ea4cf6a4aef0', N'00100000006', CAST(N'2020-08-17T19:04:36.437' AS DateTime), CAST(N'2020-08-20T19:04:36.437' AS DateTime), N'6ef26d1d-dd39-4e85-b89d-d4205fe56544', N'98c95f66-ee4f-41bc-85c2-581e794c442c', NULL, NULL, 1, CAST(N'2020-08-20T19:04:36.437' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [ParamAlicuotas] ([Id], [Codigo], [Descripcion], [Porcentaje], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'2E49EA46-6535-4173-BE7D-A35F15E03E57', N'5', N'IVA 21%', CAST(21.00 AS Numeric(18, 2)), 1, 1, NULL, NULL)
GO
INSERT [ParamAlicuotas] ([Id], [Codigo], [Descripcion], [Porcentaje], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'81b3c81c-b646-44d5-9423-cb6d5985fa45', N'4', N'IVA 10.5%', CAST(10.50 AS Numeric(18, 2)), 0, 1, NULL, NULL)
GO
INSERT [ParamAlicuotas] ([Id], [Codigo], [Descripcion], [Porcentaje], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'f073628d-08bc-4f16-8983-1daa27240cd2', N'6', N'IVA 27%', CAST(27.00 AS Numeric(18, 2)), 0, 1, NULL, NULL)
GO
INSERT [ParamCuentasCompras] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1', N'CUENTA COMPRA', 1, 1, NULL, NULL)
GO
INSERT [ParamCuentasVentas] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'1', N'CUENTA VENTA', 1, 1, NULL, NULL)
GO
INSERT [ParamUnidadesMedidas] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'07', N'UNIDAD', 1, 1, NULL, NULL)
GO
INSERT [ParamUnidadesMedidas] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'85568915-4723-4d02-bc7d-2a338b083e5d', N'98', N'OTRAS UNIDADES', 0, 1, NULL, NULL)
GO
INSERT [ParamUnidadesMedidas] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'98c2f01b-7f6e-405f-997c-c28fe8610b13', N'00', N'SIN DESCRIPCION', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposProductos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'1', N'PRODUCTOS', 1, 1, NULL, NULL)
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'0ab249f8-8a1a-4700-b02f-ae68e45f124a', N'P00100000016', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'SILLON PROMO', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(5000.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-28T12:24:18.850' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'0c2768af-7d48-4226-888a-06985e4ccb3e', N'P00100000006', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 5', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(900.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-20T17:23:01.017' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'10ef7404-8bc7-4a0c-aa17-3a6124c1af06', N'P00100000009', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 8', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(10.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-20T17:32:33.680' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'164b0cd7-839e-4eff-b761-f444005c31bd', N'P00100000007', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 6', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(900.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-20T17:31:59.753' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'1d36651d-d82c-4f52-a757-2b354fcd6936', N'P00000000004', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 3', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(200.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-19T18:02:34.110' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'2c92a740-a1ec-468f-b2e9-f2f7fdc5d52c', N'P00000000002', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 1', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(120.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-19T17:59:52.010' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'4323ab26-23ed-4e15-8104-0d4a0926b8fa', N'P00100000012', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 12', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(666.99 AS Numeric(18, 2)), 1, CAST(N'2020-08-22T08:58:20.447' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'73409929-e6df-47fb-b73d-0d8abaeec94e', N'P00100000015', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'SILLA MARA', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(120.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-28T12:23:43.100' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'92f53d68-1329-491c-bdaa-e9f1a33c4f04', N'P00100000011', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 10', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(800.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-20T17:33:11.683' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'afccbe0c-4428-46a5-96ea-44495c054300', N'P00100000010', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 9', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(325.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-20T17:32:54.490' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'bd479322-ed5c-4bf8-bde1-5a9c11b72c85', N'P00100000014', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 14', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(100.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-25T19:36:01.963' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', N'P00000000001', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'TECLADO NEGRO', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(1500.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-19T14:18:09.817' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'cdf00b83-5417-45f9-89b8-70f012f4be60', N'P00000000003', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 2', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(150.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-19T18:00:28.773' AS DateTime), N'ADMIN@REMOAR.COM.AR')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'e7ee095c-180b-44ea-8e34-761c0d4a4b3e', N'P00100000008', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 7', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(9999.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-20T17:32:17.667' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'e9ade9e5-4a11-4667-a844-d332cc31c7bd', N'P00100000013', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 13', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(889.90 AS Numeric(18, 2)), 1, CAST(N'2020-08-22T08:59:06.923' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [Productos] ([Id], [Codigo], [TipoProductoId], [Producto], [DescripcionCorta], [DescripcionLarga], [CodigoBarra], [Peso], [DimencionesLongitud], [DimencionesAncho], [DimencionesAltura], [CuentaVentaId], [CuentaCompraId], [UnidadMedidaId], [AlicuotaId], [PrecioVenta], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'ecf1f4b6-0b5b-4905-8003-2902ea53c7a4', N'P00100000005', N'25c881f6-2dea-4e20-9ea5-9c0369630672', N'PRODUCTO 4', NULL, NULL, NULL, CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), CAST(0.00 AS Numeric(18, 2)), N'd124b9b9-2bd7-4b02-b842-90b6092cb984', N'0616308a-cdf7-4f70-a193-d82f3161817e', N'1b92d7c9-96b6-463f-bbe9-8427f50bcf69', N'2E49EA46-6535-4173-BE7D-A35F15E03E57', CAST(159.00 AS Numeric(18, 2)), 1, CAST(N'2020-08-20T17:17:05.600' AS DateTime), N'MIGUEL.A.ELIAS@GMAIL.COM')
GO
INSERT [ParamConceptosIncluidos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'3619a482-f413-48ec-99bc-3860cffca8cf', N'1', N'PRODUCTO / EXPORTACIÓN DEFINITIVA DE BIENES', 0, 0, NULL, NULL)
GO
INSERT [ParamConceptosIncluidos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'4d13ccdf-09e5-4580-afad-73be8bf3f95a', N'4', N'OTRO', 0, 0, NULL, NULL)
GO
INSERT [ParamConceptosIncluidos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'84de8cb6-446f-45e7-ba8f-41b77d279f22', N'3', N'PRODUCTOS Y SERVICIOS', 1, 1, NULL, NULL)
GO
INSERT [ParamConceptosIncluidos] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'd246226e-90e7-4c1a-a2e7-4235d545784c', N'2', N'SERVICIOS', 0, 0, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'08d18173-3b22-4acd-8781-3e7010e9e07d', N'003', N'NOTAS DE CREDITO A', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'2776c74b-8998-47fb-b206-ab60312fde3f', N'008', N'NOTAS DE CREDITO B', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'32a63546-30fe-41ea-a701-c8c151dd351e', N'012', N'NOTAS DE DEBITO C', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'4ba44f46-d5eb-467b-af25-6e225293a359', N'001', N'FACTURAS A', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'5c4e5e7c-a870-477c-a24a-c79beca981f2', N'007', N'NOTAS DE DEBITO B', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'6b2dd6a9-09ce-481d-96f3-de02485520aa', N'009', N'RECIBOS B', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'814e35f3-29b4-45ee-add4-6f0aef695efb', N'011', N'FACTURAS C', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'9be07ec5-5373-4167-864b-fb2e069dbc77', N'004', N'RECIBOS A', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'af3e280d-16f1-4ffc-81f4-0bcca4a5e914', N'013', N'NOTAS DE CREDITO C', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'c0665e64-1403-40c0-8dd0-b7c2ef353e90', N'006', N'FACTURAS B', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'c1b6f099-9ac8-4e6c-b0c9-c4206c3c65cd', N'000', N'PRESPUESTOS', 1, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'c73c2faa-24b4-499a-92b2-32d3b58a8cec', N'002', N'NOTAS DE DEBITO A', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'ce6acc36-f292-4ea8-9ed1-3b4fc11fa04e', N'010', N'NOTAS DE VENTA AL CONTADO B', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'dd9d69f3-e73e-4509-9a0b-1fe7e78f3dd1', N'015', N'RECIBOS C', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'e464a852-4734-4e75-9827-1d96d81c63e0', N'005', N'NOTAS DE VENTA AL CONTADO A', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposComprobantes] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'f6f20e79-11d9-47b9-a319-f0a466dba630', N'016', N'NOTAS DE VENTA AL CONTADO C', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'02959858-3098-4fde-a798-23f74ee33ca5', N'10', N'IVA LIBERADO – LEY Nº 19.640', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'058c1651-4bd1-4d13-94d1-fd3574fc753a', N'4', N'IVA SUJETO EXENTO', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'13681254-d3ad-4c8a-920c-dedee88ab3bf', N'12', N'PEQUEÑO CONTRIBUYENTE EVENTUAL', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'321be1d9-a29e-404f-aff3-5d18c676141c', N'3', N'IVA NO RESPONSABLE', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'3c2025ce-3d47-4a67-8612-01f7a43b7218', N'14', N'PEQUEÑO CONTRIBUYENTE EVENTUAL SOCIAL', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'5f4a97dc-f8c5-4f71-8e69-1a5f515b48aa', N'6', N'RESPONSABLE MONOTRIBUTO', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'90c02915-23c1-4ba6-8be0-efb967e62f99', N'2', N'IVA RESPONSABLE NO INSCRIPTO', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'a4963af7-eb7d-4643-a7ae-6033be999285', N'1', N'IVA RESPONSABLE INSCRIPTO', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'a7e29c72-cebc-42f2-995a-2989d85e2593', N'13', N'MONOTRIBUTISTA SOCIAL', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'b4d47fe4-5493-4f91-a7b0-8ff20e8995e5', N'11', N'IVA RESPONSABLE INSCRIPTO – AGENTE DE PERCEPCIÓN', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'c2f89365-e49e-40d0-8547-fe1e13429ca7', N'9', N'CLIENTE DEL EXTERIOR', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'ce90f4be-3a79-48ac-9487-66d41dbe571c', N'5', N'CONSUMIDOR FINAL', 1, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'e37d9d98-4af9-4ffb-bb34-0414ed8bd5bd', N'8', N'PROVEEDOR DEL EXTERIOR', 0, 1, NULL, NULL)
GO
INSERT [ParamTiposResponsables] ([Id], [Codigo], [Descripcion], [Defecto], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'facdf09d-3215-42f2-b22d-fee3b0dc8fde', N'7', N'SUJETO NO CATEGORIZADO', 0, 1, NULL, NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'077f944c-6c51-4921-8ff7-77a61c394e99', N'41666916-076a-4316-b664-6cdf1e32b152', N'cdf00b83-5417-45f9-89b8-70f012f4be60', CAST(150.00 AS Numeric(18, 2)), CAST(118.50 AS Numeric(18, 2)), 3, N'admin@remoar.com.ar', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'0a57f032-bd29-40f1-b9a3-b5e34fbb92bb', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'92f53d68-1329-491c-bdaa-e9f1a33c4f04', CAST(800.00 AS Numeric(18, 2)), CAST(632.00 AS Numeric(18, 2)), 5, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'10525f55-5c41-4d63-b282-ce95c7be27b7', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'1d36651d-d82c-4f52-a757-2b354fcd6936', CAST(200.00 AS Numeric(18, 2)), CAST(158.00 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'1460a23b-ddd4-466e-ab3c-4800683ff873', N'41666916-076a-4316-b664-6cdf1e32b152', N'1d36651d-d82c-4f52-a757-2b354fcd6936', CAST(200.00 AS Numeric(18, 2)), CAST(158.00 AS Numeric(18, 2)), 1, N'admin@remoar.com.ar', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'1a91baf9-d523-4009-8587-2c0ecc45cdc1', N'4d70f28f-e568-4248-936d-71b4db2fd9e1', N'cdf00b83-5417-45f9-89b8-70f012f4be60', CAST(150.00 AS Numeric(18, 2)), CAST(118.50 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'2188e0be-556b-4085-ad4d-dff5ea514f5c', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'10ef7404-8bc7-4a0c-aa17-3a6124c1af06', CAST(10.00 AS Numeric(18, 2)), CAST(7.90 AS Numeric(18, 2)), 22, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'2bab3308-4ed0-4a9e-9b61-23e64e2154f9', N'cd1fb442-02a3-48f2-904d-36103cc59513', N'0ab249f8-8a1a-4700-b02f-ae68e45f124a', CAST(5000.00 AS Numeric(18, 2)), CAST(3950.00 AS Numeric(18, 2)), 1, N'ADMIN@REMOAR.COM.AR', CAST(N'2020-08-28T12:24:18.870' AS DateTime))
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'310055d0-fdaa-4ae1-b62e-981d58aad146', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'0c2768af-7d48-4226-888a-06985e4ccb3e', CAST(900.00 AS Numeric(18, 2)), CAST(711.00 AS Numeric(18, 2)), 2, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'41bcbb57-f922-4106-a870-d6e20e825a0b', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'cdf00b83-5417-45f9-89b8-70f012f4be60', CAST(150.00 AS Numeric(18, 2)), CAST(118.50 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'424d2c58-a5dc-4d94-8e1e-ad258eccc728', N'976CFAAC-D2EF-40E4-B8E9-3C0358E6AF1E', N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', CAST(1500.00 AS Numeric(18, 2)), CAST(1185.00 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'491f7257-fc30-4a5c-8d04-56f3eeb6b652', N'4de1bfdf-93d6-418e-a4d5-623fef5561ce', N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', CAST(1500.00 AS Numeric(18, 2)), CAST(1185.00 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'577ce8a1-3982-4b9d-81b9-a794e102481c', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'ecf1f4b6-0b5b-4905-8003-2902ea53c7a4', CAST(159.00 AS Numeric(18, 2)), CAST(125.61 AS Numeric(18, 2)), 3, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'5f70a477-442f-4f5f-83d4-98cb8f111242', N'87c93f7a-5857-4fe8-8cf3-4aacf2d5e37a', N'1d36651d-d82c-4f52-a757-2b354fcd6936', CAST(200.00 AS Numeric(18, 2)), CAST(158.00 AS Numeric(18, 2)), 6, N'MIGUEL.A.ELIAS@GMAIL.COM', CAST(N'2020-08-25T19:05:42.250' AS DateTime))
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'6a576db8-8e19-4e9d-8f51-671bc8cd3499', N'a2df9d8b-e32a-4066-b55e-10ca40658f7e', N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', CAST(1500.00 AS Numeric(18, 2)), CAST(1185.00 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'6cfa463d-2306-4b7b-b11e-ab321b313bd7', N'a2df9d8b-e32a-4066-b55e-10ca40658f7e', N'2c92a740-a1ec-468f-b2e9-f2f7fdc5d52c', CAST(120.00 AS Numeric(18, 2)), CAST(94.80 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'7908ef29-2dd5-433b-93d6-6eb43bc28f95', N'4c114592-8475-49fb-afcd-5a6ab82cca61', N'e9ade9e5-4a11-4667-a844-d332cc31c7bd', CAST(889.90 AS Numeric(18, 2)), CAST(703.02 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'9aa52877-8a4a-4806-bd52-9db6f7d159d3', N'63a35468-74f2-4175-982d-3fae31e2b436', N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', CAST(1500.00 AS Numeric(18, 2)), CAST(1185.00 AS Numeric(18, 2)), 3, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'9bf07159-c666-4659-a5c8-d2d2cf6f55c7', N'4d70f28f-e568-4248-936d-71b4db2fd9e1', N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', CAST(1500.00 AS Numeric(18, 2)), CAST(1185.00 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'a4cbae2e-cd7c-42b3-8cdb-0cb8b3dd119b', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'2c92a740-a1ec-468f-b2e9-f2f7fdc5d52c', CAST(120.00 AS Numeric(18, 2)), CAST(94.80 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'b302743e-ea87-4556-9980-8ba0aea7cf8b', N'4c114592-8475-49fb-afcd-5a6ab82cca61', N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', CAST(1500.00 AS Numeric(18, 2)), CAST(1185.00 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'b995daa7-a19e-4035-9151-c40cb48b9a11', N'41666916-076a-4316-b664-6cdf1e32b152', N'2c92a740-a1ec-468f-b2e9-f2f7fdc5d52c', CAST(120.00 AS Numeric(18, 2)), CAST(94.80 AS Numeric(18, 2)), 2, N'admin@remoar.com.ar', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'bd40993c-b4a4-45c5-a2bc-7705b446a859', N'cd1fb442-02a3-48f2-904d-36103cc59513', N'73409929-e6df-47fb-b73d-0d8abaeec94e', CAST(120.00 AS Numeric(18, 2)), CAST(94.80 AS Numeric(18, 2)), 10, N'ADMIN@REMOAR.COM.AR', CAST(N'2020-08-28T12:23:43.207' AS DateTime))
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'cf9fba20-1a2c-4b2b-a6bf-68742e0d68f1', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'164b0cd7-839e-4eff-b761-f444005c31bd', CAST(900.00 AS Numeric(18, 2)), CAST(711.00 AS Numeric(18, 2)), 2, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'd39f51dd-dc53-4416-8655-a1a552770af6', N'87c93f7a-5857-4fe8-8cf3-4aacf2d5e37a', N'bd479322-ed5c-4bf8-bde1-5a9c11b72c85', CAST(100.00 AS Numeric(18, 2)), CAST(79.00 AS Numeric(18, 2)), 15, N'MIGUEL.A.ELIAS@GMAIL.COM', CAST(N'2020-08-25T19:36:02.907' AS DateTime))
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'd8e77cb9-2a37-462a-9958-c68bbe8602b4', N'87c93f7a-5857-4fe8-8cf3-4aacf2d5e37a', N'0c2768af-7d48-4226-888a-06985e4ccb3e', CAST(900.00 AS Numeric(18, 2)), CAST(711.00 AS Numeric(18, 2)), 1, N'MIGUEL.A.ELIAS@GMAIL.COM', CAST(N'2020-08-25T19:35:39.997' AS DateTime))
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'da46d921-ab04-4dab-bece-582e6e1af7fa', N'60d45af4-f628-452d-baf1-fd8baf370dd0', N'2c92a740-a1ec-468f-b2e9-f2f7fdc5d52c', CAST(120.00 AS Numeric(18, 2)), CAST(94.80 AS Numeric(18, 2)), 2, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'e33c6b3b-0878-4935-8674-45d4361e1d2a', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'afccbe0c-4428-46a5-96ea-44495c054300', CAST(325.00 AS Numeric(18, 2)), CAST(256.75 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'e4d831ce-be0e-493c-97d7-b6436fb1dcc2', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'e7ee095c-180b-44ea-8e34-761c0d4a4b3e', CAST(9999.00 AS Numeric(18, 2)), CAST(7899.21 AS Numeric(18, 2)), 1, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'e8946c67-3874-43f0-9397-71563c89d974', N'c7ed29d7-8e95-40d7-b407-efec8d01a564', N'1d36651d-d82c-4f52-a757-2b354fcd6936', CAST(200.00 AS Numeric(18, 2)), CAST(158.00 AS Numeric(18, 2)), 2, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [PresupuestosDetalle] ([Id], [PresupuestoId], [ProductoId], [Precio], [PrecioSinImpuesto], [Cantidad], [UsuarioAlta], [FechaAlta]) VALUES (N'ef78f71f-15f8-47b8-b617-ec110d712278', N'6c0fde4c-31a4-45e0-ba10-bc059ec23330', N'bdf4db6b-a328-42df-8880-7b3e5ad23f3b', CAST(1500.00 AS Numeric(18, 2)), CAST(1185.00 AS Numeric(18, 2)), 3, N'miguel.a.elias@gmail.com', NULL)
GO
INSERT [Sucursales] ([Id], [Codigo], [Nombre], [ProvinciaId], [Localidad], [CodigoPostal], [Calle], [CalleNro], [PisoDpto], [OtrasReferencias], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'116BFA00-81FF-4317-8C5C-D9336FB61959', N'002', N'SAN JUAN', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
GO
INSERT [Sucursales] ([Id], [Codigo], [Nombre], [ProvinciaId], [Localidad], [CodigoPostal], [Calle], [CalleNro], [PisoDpto], [OtrasReferencias], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'DA5A42F8-D1F3-4369-8D7B-BB8B758156BE', N'001', N'BELGRANO', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
GO
INSERT [Sucursales] ([Id], [Codigo], [Nombre], [ProvinciaId], [Localidad], [CodigoPostal], [Calle], [CalleNro], [PisoDpto], [OtrasReferencias], [Estado], [FechaAlta], [UsuarioAlta]) VALUES (N'FE462C58-6EC3-4FC0-86E9-4FE1597D43FC', N'003', N'YERBA BUENA', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
GO
INSERT [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'38689509-066D-4882-8FE5-E4327FDE0E13', N'AMIN', N'ADMIN', NULL)
GO
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [Address], [SucursalId]) VALUES (N'32d788e0-80f5-445a-b811-fa23ad02b70f', N'miguel.a.elias@gmail.com', N'MIGUEL.A.ELIAS@GMAIL.COM', N'miguel.a.elias@gmail.com', N'MIGUEL.A.ELIAS@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAELd71w0TRpLRXIC+zM6KZX5Jt+u2FslsZCB9gSZtr2+jEYno9gaCrVFqHDaB4rjcyA==', N'R5YWSVYW3SHFN6XHLZ76VHHX3G36PMIL', N'9648d037-1a24-43d1-9a21-80686d217024', NULL, 0, 0, NULL, 1, 0, N'MIGUEL ANGEL', N'ELIAS', NULL, N'DA5A42F8-D1F3-4369-8D7B-BB8B758156BE')
GO
INSERT [AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [Address], [SucursalId]) VALUES (N'6b877fc6-5b5e-4534-97a9-e9bae58942c7', N'admin@remoar.com.ar', N'ADMIN@REMOAR.COM.AR', N'admin@remoar.com.ar', N'ADMIN@REMOAR.COM.AR', 1, N'AQAAAAEAACcQAAAAEPHV9vZEXofQ9NGl+FF6Yyn1Zd2p5KKaJ7lKgpQPcH0Isk9nf9Xrbx8LPlPJEKh7fg==', N'JFL35FVXYVHB62JNBRR2AIMRUTAXOSIF', N'90900f13-0422-451c-8d29-b09365851142', N'03815375886', 0, 0, NULL, 1, 0, N'Osvaldo ', N'Tolaba', N'Coviello 3322', N'DA5A42F8-D1F3-4369-8D7B-BB8B758156BE')
GO
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'32d788e0-80f5-445a-b811-fa23ad02b70f', N'38689509-066D-4882-8FE5-E4327FDE0E13')
GO
INSERT [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6b877fc6-5b5e-4534-97a9-e9bae58942c7', N'38689509-066D-4882-8FE5-E4327FDE0E13')
GO
INSERT [SistemaConfiguraciones] ([Id], [Configuracion], [Valor], [Estado]) VALUES (N'29fe2ff9-f84c-4886-a24a-44f148bcdf18', N'PRODUCTO_ALICUOTA_DEFAULT', N'001', 0)
GO
INSERT [SistemaConfiguraciones] ([Id], [Configuracion], [Valor], [Estado]) VALUES (N'7ccd4a9e-e6a6-4bf8-8092-15b9fe3ce6ea', N'PRESUPUESTO_APROBADO_CLIENTE', N'002', 1)
GO
INSERT [SistemaConfiguraciones] ([Id], [Configuracion], [Valor], [Estado]) VALUES (N'9778f323-1d5e-4160-9feb-bcb3a2150cc0', N'COMPROBANTES_RECIBO', N'000', 1)
GO
INSERT [SistemaConfiguraciones] ([Id], [Configuracion], [Valor], [Estado]) VALUES (N'b340ceb2-fed0-4699-b6f5-da64f97113d7', N'PRESUPUESTO_RECHAZADO_CLIENTE', N'003', 1)
GO
INSERT [SistemaConfiguraciones] ([Id], [Configuracion], [Valor], [Estado]) VALUES (N'dfeebdc6-ebad-4966-a997-a219048ae86e', N'PRESUPUESTO_PENDIENTE_CLIENTE', N'001', 1)
GO
INSERT [SistemaTablasSecuencias] ([Tabla], [Numero]) VALUES (N'CLIENTES', 4)
GO
INSERT [SistemaTablasSecuencias] ([Tabla], [Numero]) VALUES (N'PRESUPUESTOS', 48)
GO
INSERT [SistemaTablasSecuencias] ([Tabla], [Numero]) VALUES (N'PRODUCTOS', 16)
GO

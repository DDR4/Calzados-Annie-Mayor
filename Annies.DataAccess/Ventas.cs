﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Annies.Common;
using Annies.Entities;
using Dapper;

namespace Annies.DataAccess
{
    public class Ventas
    {
        public IEnumerable<Entities.Ventas> GetVentas(Entities.Ventas obj)
        {
            using (var connection = Factory.ConnectionFactory())
            {
                connection.Open();
                var parm = new DynamicParameters();                
                parm.Add("@Cod_Prod", obj.Producto.Cod_Prod);
                parm.Add("@FECHA", obj.Fecha);
                parm.Add("@Talla_Venta", obj.Talla_Venta);
                parm.Add("@Marca_Prod", obj.Producto.Marca_Prod);               
                parm.Add("@NumPagina", obj.Operacion.Inicio);
                parm.Add("@TamPagina", obj.Operacion.Fin);

                var result = connection.Query(
                     sql: "SP_BUSCAR_VENTA",
                     param: parm,
                     commandType: CommandType.StoredProcedure)
                     .Select(m => m as IDictionary<string, object>)
                     .Select(n => new Entities.Ventas
                     {
                         Cod_Venta = n.Single(d => d.Key.Equals("Cod_Venta")).Value.Parse<int>(),
                         Producto = new Annies.Entities.Producto
                         {
                             Cod_Prod = n.Single(d => d.Key.Equals("Cod_Prod")).Value.Parse<int>(),
                             Marca_Prod = n.Single(d => d.Key.Equals("Marca_Prod")).Value.Parse<string>(),
                         },
                         Precio_Venta = n.Single(d => d.Key.Equals("Precio_Venta")).Value.Parse<int>(),
                         Precio_Final = n.Single(d => d.Key.Equals("Precio_Final")).Value.Parse<int>(),
                         Talla_Venta = n.Single(d => d.Key.Equals("Talla_Venta")).Value.Parse<string>(),
                         Cant_Venta = n.Single(d => d.Key.Equals("Cant_Venta")).Value.Parse<int>(),
                         Fecha = n.Single(d => d.Key.Equals("Fecha")).Value.Parse<int>(),
                         Auditoria = new Entities.Auditoria
                         {
                             TipoUsuario = obj.Auditoria.TipoUsuario
                         },
                         Operacion = new Entities.Operacion
                         {
                             TotalRows = n.Single(d=> d.Key.Equals("TotalRows")).Value.Parse<int>()
                         }
                     });

                return result;
            }
        }

        public int InsertUpdateVentas(Entities.Ventas obj)
        {

            using (var connection = Factory.ConnectionFactory())
            {
                connection.Open();
                var parm = new DynamicParameters();
                parm.Add("@Cod_Prod", obj.Producto.Cod_Prod);
                parm.Add("@Talla_Venta", obj.Talla_Venta);
                parm.Add("@Cant_Venta", obj.Cant_Venta);
                parm.Add("@Descuento_Venta", obj.Descuento_Venta);
                parm.Add("@Precio_Venta", obj.Producto.Precio_Prod);
                parm.Add("@FECHA", obj.Fecha);
                parm.Add("@Usuario", obj.Auditoria.UsuarioCreacion);           

                var result = connection.Execute(
                    sql: "SP_INSERTAR_VENTA",
                    param: parm,
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public int DeleteVentas(Entities.Ventas obj)
        {

            using (var connection = Factory.ConnectionFactory())
            {
                connection.Open();
                var parm = new DynamicParameters();
                parm.Add("@Cod_Venta", obj.Cod_Venta);
                parm.Add("@Usuario", obj.Auditoria.UsuarioModificacion);

                var result = connection.Execute(
                    sql: "SP_ELIMINAR_VENTA",
                    param: parm,
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public IEnumerable<Entities.Ventas> GetAllVentas(Entities.Ventas obj)
        {
            using (var connection = Factory.ConnectionFactory())
            {
                connection.Open();
                var parm = new DynamicParameters();
                parm.Add("@Cod_Prod", obj.Producto.Cod_Prod);
                parm.Add("@Marca_Prod", obj.Producto.Marca_Prod);
                parm.Add("@Talla_Venta", obj.Talla_Venta);
                parm.Add("@FechaDesde", obj.FechaDesde);
                parm.Add("@FechaHasta", obj.FechaHasta);

                var result = connection.Query(
                     sql: "SP_FILTRAR_VENTA",
                     param: parm,
                     commandType: CommandType.StoredProcedure)
                     .Select(m => m as IDictionary<string, object>)
                          .Select(n => new Entities.Ventas
                          {
                              Cod_Venta = n.Single(d => d.Key.Equals("Cod_Venta")).Value.Parse<int>(),
                              Producto = new Annies.Entities.Producto
                              {
                                  Cod_Prod = n.Single(d => d.Key.Equals("Cod_Prod")).Value.Parse<int>(),
                                  Marca_Prod = n.Single(d => d.Key.Equals("Marca_Prod")).Value.Parse<string>(),
                              },
                              Cant_Venta = n.Single(d => d.Key.Equals("Cant_Venta")).Value.Parse<int>(),
                              Fecha = n.Single(d => d.Key.Equals("Fecha")).Value.Parse<int>(),
                              Precio_Venta = n.Single(d => d.Key.Equals("Precio_Venta")).Value.Parse<int>(),
                              Precio_Final = n.Single(d => d.Key.Equals("Precio_Final")).Value.Parse<int>(),
                              Talla_Venta = n.Single(d => d.Key.Equals("Talla_Venta")).Value.Parse<string>()                          
                         
                          });

                return result;
            }
        }

        public IEnumerable<Entities.Producto> GetProducto(Entities.Producto obj)
        {
            using (var connection = Factory.ConnectionFactory())
            {
                connection.Open();
                var parm = new DynamicParameters();
                parm.Add("@Cod_Prod", obj.Cod_Prod);
                parm.Add("@Marca_Prod", obj.Marca_Prod);
                parm.Add("@Stock_Prod", obj.Stock_Prod);
                parm.Add("@NumPagina", obj.Operacion.Inicio);
                parm.Add("@TamPagina", obj.Operacion.Fin);
                var result = connection.Query(
                     sql: "SP_BUSCAR_PRODUCTO_VENTAS",
                     param: parm,
                     commandType: CommandType.StoredProcedure)
                     .Select(m => m as IDictionary<string, object>)
                     .Select(n => new Entities.Producto
                     {
                         Cod_Prod = n.Single(d => d.Key.Equals("Cod_Prod")).Value.Parse<int>(),
                         Marca_Prod = n.Single(d => d.Key.Equals("Marca_Prod")).Value.Parse<string>(),
                         Precio_Prod = n.Single(d => d.Key.Equals("Precio_Prod")).Value.Parse<double>(),
                         Stock_Prod = n.Single(d => d.Key.Equals("Stock_Prod")).Value.Parse<int>(),
                         Codigo_Al = n.Single(d => d.Key.Equals("Cod_Almacen")).Value.Parse<string>(),
                         Estado_Prod = n.Single(d => d.Key.Equals("Estado_Prod")).Value.Parse<int>(),
                         Auditoria = new Auditoria
                         {
                             TipoUsuario = obj.Auditoria.TipoUsuario,
                         },
                         Operacion = new Operacion
                         {
                             TotalRows = n.Single(d => d.Key.Equals("TotalRows")).Value.Parse<int>(),
                         }
                     });

                return result;
            }
        }

    }
}

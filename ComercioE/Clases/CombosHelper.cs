﻿using ComercioE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace ComercioE.Clases
{
    public class CombosHelper : IDisposable
    {
        private static ComercioEContext db = new ComercioEContext();
        public static List<Provincia> GetProvincias()
        {
            var provincias = db.Provincias.ToList();
            provincias.Add(new Provincia
            {
                ProvinciaId = 0,
                Nombre = "[Seleccione una Provincia]",
            });
            return provincias.OrderBy(p => p.Nombre).ToList();
        }

        public static List<Ciudad> GetCiudades(int provinciaId)
        {
            var ciudades = db.Ciudads.Where( c=> c.ProvinciaId == provinciaId).ToList();
            ciudades.Add(new Ciudad
            {
                CiudadId = 0,
                Nombre = "[Seleccione una Ciudad]",
            });
            return ciudades.OrderBy(p => p.Nombre).ToList();
        }

        //public static List<Ciudad> GetCiudades()
        //{
        //    var ciudades = db.Ciudads.ToList();
        //    ciudades.Add(new Ciudad
        //    {
        //        CiudadId = 0,
        //        Nombre = "[Seleccione una Ciudad]",
        //    });
        //    return ciudades.OrderBy(p => p.Nombre).ToList();
        //}

        public static List<Producto> GetProductos(int companyId, bool ola)
        {
            var productos = db.Productoes.Where(p => p.CompaniaId == companyId).ToList();
            return productos.OrderBy(p => p.Descripcion).ToList();
        }

        public static List<Producto> GetProductos(int companiaId)
        {
            var productos = db.Productoes.Where(p => p.CompaniaId == companiaId).ToList();
            productos.Add(new Producto
            {
                ProductoId = 0,
                Descripcion = "[Seleccione Producto]",
            });
            return productos.OrderBy(p => p.Descripcion).ToList();
        }

        public static List<Compania> GetCompanias()
        {
            var companias = db.Companias.ToList();
            companias.Add(new Compania
            {
                CompaniaId = 0,
                Nombre = "[Seleccione Compañia]",
            });
            return companias.OrderBy(p => p.Nombre).ToList();
        }
        public void Dispose()
        {
            db.Dispose();
        }

        public static List<Categoria> GetCategorias(int companiaId)
        {
            var categorias = db.Categorias.Where( c => c.CompaniaId == companiaId).ToList();
            categorias.Add(new Categoria
            {
                CategoriaId = 0,
                Descripcion = "[Seleccione categoria]",
            });
            return categorias.OrderBy(p => p.Descripcion).ToList();
        }

        public static List<Cliente> GetClientes(int companiaId)
        {
            var clientes = db.Clientes.Where(c => c.CompaniaId == companiaId).ToList();
            clientes.Add(new Cliente
            {
                ClienteId = 0,
                Nombre = "[Seleccione Cliente]",
            });
            return clientes.OrderBy(c => c.Nombre).ThenBy(c=> c.Apellido).ToList();
        }

        public static List<Impuesto> GetImpuestos(int companiaId)
        {
            var impuestos = db.Impuestoes.Where(c => c.CompaniaId == companiaId).ToList();
            impuestos.Add(new Impuesto
            {
                ImpuestoId = 0,
                Descripcion = "[Seleccione un Impuesto]",
            });
            return impuestos.OrderBy(p => p.Descripcion).ToList();
        }
    }
}
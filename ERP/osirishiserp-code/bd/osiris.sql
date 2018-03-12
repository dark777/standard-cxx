--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = off;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET escape_string_warning = off;

SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: osiris_erp_tipo_comprobante; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_tipo_comprobante (
    id_tipo_comprobante integer NOT NULL,
    descripcion_tipo_comprobante character varying DEFAULT ''::bpchar,
    activo boolean DEFAULT true
);


ALTER TABLE public.osiris_erp_tipo_comprobante OWNER TO admin;

--
-- Name: TABLE osiris_erp_tipo_comprobante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_tipo_comprobante IS 'tipos de comprobante';


--
-- Name: erp_osiris_tipo_comprobante_id_tipo_comprobante_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE erp_osiris_tipo_comprobante_id_tipo_comprobante_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.erp_osiris_tipo_comprobante_id_tipo_comprobante_seq OWNER TO admin;

--
-- Name: erp_osiris_tipo_comprobante_id_tipo_comprobante_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE erp_osiris_tipo_comprobante_id_tipo_comprobante_seq OWNED BY osiris_erp_tipo_comprobante.id_tipo_comprobante;


--
-- Name: osiris_almacenes; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_almacenes (
    id_almacen integer DEFAULT nextval(('public.osiris_almacenes_id_almacen_seq'::text)::regclass),
    descripcion_almacen character varying,
    sub_almacen boolean DEFAULT true,
    id_tipo_admisiones integer,
    almacen_salidas boolean DEFAULT false,
    activo boolean DEFAULT true,
    pre_solicitudes boolean DEFAULT false,
    acceso_grupo_producto character varying DEFAULT ''::bpchar,
    cargo_directo boolean DEFAULT false,
    cargo_directo_requisicion boolean DEFAULT false,
    requiere_autorizacion boolean DEFAULT false
);


ALTER TABLE public.osiris_almacenes OWNER TO admin;

--
-- Name: TABLE osiris_almacenes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_almacenes IS 'Esta tabla almacena los almacenes o sub-almacenes que existen en el hospital';


--
-- Name: COLUMN osiris_almacenes.id_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.id_almacen IS 'identifica almacen';


--
-- Name: COLUMN osiris_almacenes.descripcion_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.descripcion_almacen IS 'almacena la descripcion del almacen';


--
-- Name: COLUMN osiris_almacenes.sub_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.sub_almacen IS 'me indica si este almacen es subalmacen para hacer traspasos';


--
-- Name: COLUMN osiris_almacenes.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.id_tipo_admisiones IS 'este campo se enlasa con la table his_tipo_admisiones centros de costos';


--
-- Name: COLUMN osiris_almacenes.almacen_salidas; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.almacen_salidas IS 'este campo me indica si son alamcenes de salidads de materiales hacia sub-almacenes';


--
-- Name: COLUMN osiris_almacenes.pre_solicitudes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.pre_solicitudes IS 'permite que este almacen pueda realizar pre solicitudes hacia almacen';


--
-- Name: COLUMN osiris_almacenes.acceso_grupo_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.acceso_grupo_producto IS 'almacena los numero de los grupos de productos que tiene acceso este almacen';


--
-- Name: COLUMN osiris_almacenes.cargo_directo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.cargo_directo IS 'esta bandera me permite saber si el almacen general es el que realizara el cargo a la hora de surtir';


--
-- Name: COLUMN osiris_almacenes.cargo_directo_requisicion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.cargo_directo_requisicion IS 'bandera que me indicara que el producto que se recibe en almacen general se carga directo cuan es por paciente';


--
-- Name: COLUMN osiris_almacenes.requiere_autorizacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_almacenes.requiere_autorizacion IS 'bandera que indica si el este almacen requiere que sea autorizada la solicitud de materiales';


--
-- Name: osiris_almacenes_id_almacen_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_almacenes_id_almacen_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_almacenes_id_almacen_seq OWNER TO admin;

--
-- Name: osiris_aseguradoras; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_aseguradoras (
    descripcion_aseguradora character varying(60) DEFAULT ''::bpchar,
    activa boolean,
    nombre_contacto character varying(60) DEFAULT ''::bpchar,
    cargo_contacto character varying(30) DEFAULT ''::bpchar,
    telefono_trabajo1_contacto character varying(15) DEFAULT ''::bpchar,
    id_aseguradora integer NOT NULL,
    lista_de_precio boolean DEFAULT false,
    id_tipo_documento integer DEFAULT 1,
    id_tipo_paciente integer DEFAULT 0
);


ALTER TABLE public.osiris_aseguradoras OWNER TO admin;

--
-- Name: TABLE osiris_aseguradoras; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_aseguradoras IS 'Almacena todas las compañias aseguradoras que tienen convenio';


--
-- Name: COLUMN osiris_aseguradoras.activa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_aseguradoras.activa IS 'activa o desactiva la compañia aseguradora';


--
-- Name: COLUMN osiris_aseguradoras.nombre_contacto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_aseguradoras.nombre_contacto IS 'almacena el nombre del contacto del la aseguradora';


--
-- Name: COLUMN osiris_aseguradoras.cargo_contacto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_aseguradoras.cargo_contacto IS 'que cargo tiene dentro de la empresa el contacto';


--
-- Name: COLUMN osiris_aseguradoras.id_aseguradora; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_aseguradoras.id_aseguradora IS 'compañia aseguradora, numero correlativo';


--
-- Name: COLUMN osiris_aseguradoras.lista_de_precio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_aseguradoras.lista_de_precio IS 'este campo me indica si este cliente tiene una lista de precio para el enlase de precios';


--
-- Name: COLUMN osiris_aseguradoras.id_tipo_documento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_aseguradoras.id_tipo_documento IS 'este campo se enlasa con la tabla de tipos de documentos';


--
-- Name: osiris_aseguradoras_id_aseguradora_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_aseguradoras_id_aseguradora_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_aseguradoras_id_aseguradora_seq OWNER TO admin;

--
-- Name: osiris_aseguradoras_id_aseguradora_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_aseguradoras_id_aseguradora_seq OWNED BY osiris_aseguradoras.id_aseguradora;


--
-- Name: osiris_catalogo_almacen_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_catalogo_almacen_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_catalogo_almacen_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_catalogo_almacenes; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_catalogo_almacenes (
    id_almacen integer,
    maximo numeric(10,3) DEFAULT 0,
    minimo_stock numeric(10,3) DEFAULT 0,
    punto_de_reorden numeric(10,3) DEFAULT 0,
    stock numeric(10,3) DEFAULT 0,
    fechahora_alta timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15),
    id_producto numeric(12,0),
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying,
    fechahora_ultimo_surtimiento timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_secuencia integer NOT NULL,
    historial_ajustes text DEFAULT ''::bpchar,
    tiene_stock boolean DEFAULT false
);


ALTER TABLE public.osiris_catalogo_almacenes OWNER TO admin;

--
-- Name: TABLE osiris_catalogo_almacenes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_catalogo_almacenes IS 'Esta tabla almacena todos los catalogos de productos asignados por almacen';


--
-- Name: COLUMN osiris_catalogo_almacenes.id_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.id_almacen IS 'se enlasa con la tabla almacenes';


--
-- Name: COLUMN osiris_catalogo_almacenes.maximo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.maximo IS 'almacena el maximo de inventario que debe tener ese producto';


--
-- Name: COLUMN osiris_catalogo_almacenes.minimo_stock; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.minimo_stock IS 'minimo que debe haber en almacen';


--
-- Name: COLUMN osiris_catalogo_almacenes.punto_de_reorden; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.punto_de_reorden IS 'almacena el punto de reorden';


--
-- Name: COLUMN osiris_catalogo_almacenes.stock; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.stock IS 'contiene el numero actual que hay en almacen';


--
-- Name: COLUMN osiris_catalogo_almacenes.fechahora_alta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.fechahora_alta IS 'almacena la fecha ya lhora que almacen asigo ese producto al sub-almacen';


--
-- Name: COLUMN osiris_catalogo_almacenes.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.id_quien_creo IS 'almacena quien hizo la alta a este sub-almacen';


--
-- Name: COLUMN osiris_catalogo_almacenes.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.id_producto IS 'el codigo de producto enlasado a la tabla de productos';


--
-- Name: COLUMN osiris_catalogo_almacenes.fechahora_ultimo_surtimiento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.fechahora_ultimo_surtimiento IS 'alamcena la fecha del ultimo surtimiento de material';


--
-- Name: COLUMN osiris_catalogo_almacenes.historial_ajustes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.historial_ajustes IS 'guarda el historial de ajustes de inventario';


--
-- Name: COLUMN osiris_catalogo_almacenes.tiene_stock; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_almacenes.tiene_stock IS 'este campo indica si el producto tendra un stock en el subalamcen';


--
-- Name: osiris_catalogo_almacenes_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_catalogo_almacenes_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_catalogo_almacenes_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_catalogo_almacenes_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_catalogo_almacenes_id_secuencia_seq OWNED BY osiris_catalogo_almacenes.id_secuencia;


--
-- Name: osiris_catalogo_productos_proveedores; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_catalogo_productos_proveedores (
    id_secuencia integer NOT NULL,
    id_producto numeric(12,0) DEFAULT 0,
    id_proveedor integer DEFAULT 1,
    precio_costo numeric(13,5) DEFAULT 0,
    precio_costo_unitario numeric(13,5) DEFAULT 0,
    cantidad_de_embalaje numeric(6,2) DEFAULT 0,
    descripcion_producto character varying DEFAULT ''::bpchar,
    historial_cambios text DEFAULT ''::bpchar,
    tipo_unidad_producto character varying(15) DEFAULT ''::bpchar,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    codigo_producto_proveedor character varying DEFAULT ''::bpchar,
    codigo_de_barra character varying DEFAULT ''::bpchar,
    clave character varying(35) DEFAULT ''::bpchar,
    eliminado boolean DEFAULT false,
    id_quien_asigno_osiris character varying(15) DEFAULT ''::bpchar,
    fecha_asigno_osiris timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15) DEFAULT ''::bpchar,
    fecha_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    descripcion_producto_osiris character varying DEFAULT ''::bpchar,
    marca_producto character varying DEFAULT ''::bpchar,
    historial_movimientos text DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_catalogo_productos_proveedores OWNER TO admin;

--
-- Name: TABLE osiris_catalogo_productos_proveedores; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_catalogo_productos_proveedores IS 'almacena todos los catalogo de productos de los proveedores';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.id_producto IS 'este campo se enlasa con la tabla osiris_productos';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.id_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.id_proveedor IS 'este campo se enlasa con tabla osiris_erp_proveedores';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.historial_cambios; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.historial_cambios IS 'almacen las variaciones de precio que tiene el producto';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.tipo_unidad_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.tipo_unidad_producto IS 'este campo nos indica si el el producto es por PZ  KG.  LTs. etc';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.fechahora_creacion IS 'guadar la fecha y la hora de creacion del producto';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.id_quien_creo IS 'almacen el id del usuario quien dio de alta el producto';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.codigo_producto_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.codigo_producto_proveedor IS 'almacena el codigo interno que tiene el proveedor';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.codigo_de_barra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.codigo_de_barra IS 'alamcena el codigo de barra del producto';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.id_quien_elimino; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.id_quien_elimino IS 'almacena quien cancelo producto ya guardado';


--
-- Name: COLUMN osiris_catalogo_productos_proveedores.marca_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_catalogo_productos_proveedores.marca_producto IS 'marca del producto que ofrece el proveedor';


--
-- Name: osiris_catalogo_productos_proveedores_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_catalogo_productos_proveedores_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_catalogo_productos_proveedores_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_catalogo_productos_proveedores_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_catalogo_productos_proveedores_id_secuencia_seq OWNED BY osiris_catalogo_productos_proveedores.id_secuencia;


--
-- Name: osiris_empleado; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_empleado (
    id_empleado character varying(7) DEFAULT ''::bpchar,
    login_empleado character varying(15) DEFAULT ''::bpchar,
    nombre1_empleado character varying(20),
    nombre2_empleado character varying(20),
    apellido_paterno_empleado character varying(20),
    apellido_materno_empleado character varying(20),
    password_empleado character varying,
    acceso_osiris boolean DEFAULT false,
    baja_empleado boolean DEFAULT false NOT NULL,
    fechahora_contrato_captura timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fechahora_ingreso_captura timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    historial_de_contrato text DEFAULT ''::bpchar,
    historial_de_cambios text DEFAULT ''::bpchar,
    acceso_his character varying DEFAULT '000000000000000000000'::bpchar,
    acceso_erp character varying DEFAULT '000000000000000000000'::bpchar,
    acceso_general character varying DEFAULT '111100000000000000000'::bpchar,
    autoriza_his boolean DEFAULT false,
    autoriza_erp boolean DEFAULT false,
    autoriza_general boolean DEFAULT false,
    estado_del_empleado character varying(1) DEFAULT 0,
    foto_empleado oid,
    nombre_imagen text DEFAULT ''::bpchar,
    accseso_quirofano boolean DEFAULT false,
    id_sucursal integer DEFAULT 1,
    acceso_abrir_folio boolean DEFAULT false,
    acceso_catalogo_producto boolean DEFAULT false,
    acceso_cx_pq boolean DEFAULT false,
    acceso_cancelar_folio boolean DEFAULT false,
    acceso_crearpase_qxurg boolean DEFAULT false,
    acceso_imprimirpase_qxurg boolean DEFAULT false,
    acceso_eliminarpase_qxurg boolean DEFAULT false,
    exportar_compserv boolean DEFAULT false,
    exportar_pasesqx boolean DEFAULT false,
    acceso_catalogo_doctores boolean DEFAULT false,
    exportar_pagares boolean DEFAULT false,
    exportar_cortecaja boolean DEFAULT false,
    exportar_proc_cobranza boolean DEFAULT false,
    acceso_cancela_abopago boolean DEFAULT false,
    acceso_ajuste_inv boolean DEFAULT false,
    acceso_traspasos_inv boolean DEFAULT false,
    acceso_cancela_pagare boolean DEFAULT false
);


ALTER TABLE public.osiris_empleado OWNER TO admin;

--
-- Name: TABLE osiris_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_empleado IS 'Almacena toda la informacion de lo empleados que trabajan o que trabajaron';


--
-- Name: COLUMN osiris_empleado.acceso_osiris; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_osiris IS 'Permite a acceso al sistema osiris, esto lo actualiza el administrador del sistema';


--
-- Name: COLUMN osiris_empleado.baja_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.baja_empleado IS 'false = no baja, true = baja';


--
-- Name: COLUMN osiris_empleado.fechahora_contrato_captura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.fechahora_contrato_captura IS 'almacena cuando se realizao la operacion';


--
-- Name: COLUMN osiris_empleado.acceso_his; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_his IS 'control de accesos para his';


--
-- Name: COLUMN osiris_empleado.acceso_erp; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_erp IS 'control de accesos para erp';


--
-- Name: COLUMN osiris_empleado.acceso_general; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_general IS 'control de acceso general para usuarios';


--
-- Name: COLUMN osiris_empleado.foto_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.foto_empleado IS 'foto del empleado';


--
-- Name: COLUMN osiris_empleado.nombre_imagen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.nombre_imagen IS 'almacen el nombre de la imagen';


--
-- Name: COLUMN osiris_empleado.accseso_quirofano; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.accseso_quirofano IS 'esta bandera indica accseso al empleado de quirofano';


--
-- Name: COLUMN osiris_empleado.id_sucursal; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.id_sucursal IS 'se enlasa con la tabla de sucursales asignadas a los empleados';


--
-- Name: COLUMN osiris_empleado.acceso_catalogo_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_catalogo_producto IS 'bandera que indica si puede accesar al catalogo de productos';


--
-- Name: COLUMN osiris_empleado.acceso_cx_pq; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_cx_pq IS 'bandera que indica si tiene acceso a crea cirugias o paquetes quirurgicos';


--
-- Name: COLUMN osiris_empleado.acceso_cancelar_folio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_cancelar_folio IS 'bandera que indica si puede cancelar numero de atencion';


--
-- Name: COLUMN osiris_empleado.acceso_crearpase_qxurg; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_crearpase_qxurg IS 'otorga el acceso para que pueda generar un pase medico a qx urg';


--
-- Name: COLUMN osiris_empleado.acceso_imprimirpase_qxurg; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_imprimirpase_qxurg IS 'acceso para poder imprimir los pases de quirofano';


--
-- Name: COLUMN osiris_empleado.acceso_eliminarpase_qxurg; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_eliminarpase_qxurg IS 'otorga permiso para poder eliminar un pase qx o urgencias';


--
-- Name: COLUMN osiris_empleado.exportar_compserv; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.exportar_compserv IS 'permiso para poder exportar comprobantes de servicio a una hoja de calculo .ods';


--
-- Name: COLUMN osiris_empleado.exportar_pasesqx; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.exportar_pasesqx IS 'permiso para poder exportar pases de quirofano urg a una hoja de calculo .ods';


--
-- Name: COLUMN osiris_empleado.acceso_catalogo_doctores; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_catalogo_doctores IS 'acceso al catalogo de doctores';


--
-- Name: COLUMN osiris_empleado.exportar_pagares; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.exportar_pagares IS 'acceso para exportar los pagares';


--
-- Name: COLUMN osiris_empleado.exportar_cortecaja; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.exportar_cortecaja IS 'acceso para exportar los cortes de caja';


--
-- Name: COLUMN osiris_empleado.exportar_proc_cobranza; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.exportar_proc_cobranza IS 'Exporta el procedimiento quirurgico a una hoja de calculo ods';


--
-- Name: COLUMN osiris_empleado.acceso_cancela_abopago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_cancela_abopago IS 'acceso para cancelar un abono o pago en caja';


--
-- Name: COLUMN osiris_empleado.acceso_ajuste_inv; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_ajuste_inv IS 'acceso para poder realizar ajustes de inventario';


--
-- Name: COLUMN osiris_empleado.acceso_traspasos_inv; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_traspasos_inv IS 'acceso para poder realizar traspasos entre sub-almacenes';


--
-- Name: COLUMN osiris_empleado.acceso_cancela_pagare; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado.acceso_cancela_pagare IS 'permiso para poder cancelar un pagare';


--
-- Name: osiris_empleado_detalle; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_empleado_detalle (
    id_empleado character varying(7),
    calle character varying(35) DEFAULT ''::character varying,
    numero character varying(7) DEFAULT ''::character varying,
    colonia character varying(40) DEFAULT ''::character varying,
    municipio character varying(30) DEFAULT ''::character varying,
    estado character varying(30) DEFAULT ''::character varying,
    codigo_postal integer DEFAULT 0,
    telefono_1 character varying(20) DEFAULT ''::character varying,
    telefono_2 character varying(20) DEFAULT ''::character varying,
    email1 character varying(35) DEFAULT ''::character varying,
    email2 character varying(35) DEFAULT ''::character varying,
    celular character varying(20) DEFAULT ''::character varying,
    fax character varying(20) DEFAULT ''::character varying,
    rfc character varying(13) DEFAULT ''::character varying,
    curp character varying(18) DEFAULT ''::character varying,
    imss character varying(11) DEFAULT ''::character varying,
    infonavit character varying(20) DEFAULT ''::character varying,
    afore character varying(35) DEFAULT ''::character varying,
    cartilla_militar character varying(20) DEFAULT ''::character varying,
    escolaridad character varying(30) DEFAULT ''::character varying,
    titulo character varying(35) DEFAULT ''::character varying,
    religion character varying(20) DEFAULT ''::character varying,
    residencia integer DEFAULT 0,
    nombre_padre character varying(80) DEFAULT ''::character varying,
    nombre_madre character varying(80) DEFAULT ''::character varying,
    nombre_conyuge character varying(80) DEFAULT ''::character varying,
    num_hijos integer DEFAULT 0,
    fecha_de_nacimiento date DEFAULT '2000-01-01'::date,
    fecha_de_ingreso date DEFAULT '2000-01-01'::date,
    fecha_de_contrato date DEFAULT '2000-01-01'::date,
    fecha_de_baja date DEFAULT '2000-01-01'::date,
    estado_civil character varying(20) DEFAULT ''::character varying,
    lugar_nacimiento character varying(80) DEFAULT ''::character varying,
    nacionalidad character varying(20) DEFAULT 'Mexicana'::character varying,
    talla_pantalon character varying(10) DEFAULT ''::character varying,
    talla_chaqueta character varying(10) DEFAULT ''::character varying,
    talla_zapatos character varying(10) DEFAULT ''::character varying,
    peso_empleado numeric(6,2) DEFAULT 0.0,
    estatura_empleado numeric(5,2) DEFAULT 0.0,
    tipo_sangre character varying(5) DEFAULT ''::character varying,
    accidente_avisar_a character varying(80) DEFAULT ''::character varying,
    accidente_telefono character varying(20) DEFAULT ''::character varying,
    sueldo_actual numeric(13,2) DEFAULT 0.0,
    sueldo_min numeric(13,2) DEFAULT 0.0,
    sueldo_max numeric(13,2) DEFAULT 0.0,
    causa_baja character(30) DEFAULT 0,
    notas_baja character varying(100) DEFAULT ''::character varying,
    notas_empleado character varying(300) DEFAULT ''::character varying,
    dias_vac_goce integer DEFAULT 0,
    dias_vac_gozados integer DEFAULT 0,
    dias_vac_por_disfrutar integer DEFAULT 0,
    genero character varying(1) DEFAULT ''::character varying,
    tipo_casa character varying(20) DEFAULT ''::character varying,
    tipo_empleado character varying(20) DEFAULT ''::character varying,
    tipo_contrato character varying(30) DEFAULT ''::character varying,
    tipo_pago character varying(30) DEFAULT ''::character varying,
    jornada character varying(20) DEFAULT ''::character varying,
    departamento character varying(50) DEFAULT ''::character varying,
    puesto character varying(60) DEFAULT ''::character varying,
    id_puesto integer DEFAULT 0,
    id_departamento integer DEFAULT 0,
    cedula_profesional integer DEFAULT 0,
    num_lockers character varying(10) DEFAULT ''::character varying,
    tiempo_comida character varying(20) DEFAULT ''::character varying,
    tipo_funcion character varying(30) DEFAULT ''::character varying,
    historial_de_contrato text DEFAULT ''::bpchar,
    historial_de_cambios text DEFAULT ''::bpchar,
    fecha_operacion date DEFAULT '2000-01-01'::date,
    contrato_indeterminado boolean DEFAULT false,
    fecha_vencimiento_contrato date DEFAULT '2000-01-01'::date,
    fecha_de_registro timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_secuencia integer NOT NULL
);


ALTER TABLE public.osiris_empleado_detalle OWNER TO admin;

--
-- Name: TABLE osiris_empleado_detalle; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_empleado_detalle IS 'Almacena el detalle de los datos del empleado';


--
-- Name: COLUMN osiris_empleado_detalle.infonavit; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.infonavit IS 'Almacena el numero de credito de infonavit del trabajador';


--
-- Name: COLUMN osiris_empleado_detalle.afore; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.afore IS 'Almacena el nombre del banco o numero de afore';


--
-- Name: COLUMN osiris_empleado_detalle.escolaridad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.escolaridad IS 'Nivel maximo de estudios';


--
-- Name: COLUMN osiris_empleado_detalle.titulo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.titulo IS 'titulo obtenido por el empleado';


--
-- Name: COLUMN osiris_empleado_detalle.residencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.residencia IS 'numero de años de residencia en su domicilio';


--
-- Name: COLUMN osiris_empleado_detalle.fecha_de_contrato; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.fecha_de_contrato IS 'Desde que fecha esta vigente su contrato actual';


--
-- Name: COLUMN osiris_empleado_detalle.fecha_de_baja; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.fecha_de_baja IS 'fecha en que aplica la baja';


--
-- Name: COLUMN osiris_empleado_detalle.peso_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.peso_empleado IS 'kg';


--
-- Name: COLUMN osiris_empleado_detalle.estatura_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.estatura_empleado IS 'metros';


--
-- Name: COLUMN osiris_empleado_detalle.dias_vac_goce; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.dias_vac_goce IS 'Dias que le corresponden en el año actual (politica)';


--
-- Name: COLUMN osiris_empleado_detalle.dias_vac_gozados; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.dias_vac_gozados IS 'dias que ya ha gozado';


--
-- Name: COLUMN osiris_empleado_detalle.dias_vac_por_disfrutar; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.dias_vac_por_disfrutar IS 'dias por disfrutar de su año actual =(diasgoce-disfrutados)';


--
-- Name: COLUMN osiris_empleado_detalle.genero; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.genero IS 'H=Hombre, M=Mujer';


--
-- Name: COLUMN osiris_empleado_detalle.tipo_casa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.tipo_casa IS 'rentada, propia, padres, etc';


--
-- Name: COLUMN osiris_empleado_detalle.tipo_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.tipo_empleado IS 'N=Empleado, E=Extero, P=Practicas, T=Temporal,';


--
-- Name: COLUMN osiris_empleado_detalle.tipo_contrato; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.tipo_contrato IS 'Almacena el tipo de contrato';


--
-- Name: COLUMN osiris_empleado_detalle.tipo_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.tipo_pago IS 'Almacena el tipo de pago';


--
-- Name: COLUMN osiris_empleado_detalle.jornada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.jornada IS 'Jornada de trabajo';


--
-- Name: COLUMN osiris_empleado_detalle.departamento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.departamento IS 'almacena clave y descripcion del departamento';


--
-- Name: COLUMN osiris_empleado_detalle.puesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.puesto IS 'almacena clave y descripcion del puesto';


--
-- Name: COLUMN osiris_empleado_detalle.id_puesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.id_puesto IS 'este campo se enlaza  con la tabla erp_puestos';


--
-- Name: COLUMN osiris_empleado_detalle.cedula_profesional; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.cedula_profesional IS 'numero de cedula profesional de los quimicos';


--
-- Name: COLUMN osiris_empleado_detalle.num_lockers; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.num_lockers IS 'numero del locker';


--
-- Name: COLUMN osiris_empleado_detalle.contrato_indeterminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empleado_detalle.contrato_indeterminado IS 'verifica si tiene contrato indterminado';


--
-- Name: osiris_empleado_detalle_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_empleado_detalle_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_empleado_detalle_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_empleado_detalle_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_empleado_detalle_id_secuencia_seq OWNED BY osiris_empleado_detalle.id_secuencia;


--
-- Name: osiris_empleados_accesos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_empleados_accesos (
    id integer NOT NULL,
    login_empleado character varying(15) DEFAULT ''::bpchar,
    id_empleado character varying(7) DEFAULT ''::bpchar,
    fechahora_login timestamp without time zone DEFAULT '2000-01-01 00:00:00'::timestamp without time zone
);


ALTER TABLE public.osiris_empleados_accesos OWNER TO admin;

--
-- Name: TABLE osiris_empleados_accesos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_empleados_accesos IS 'Esta tabla graba todas las entradas o accesos que han hecho al sistema';


--
-- Name: osiris_empleados_accesos_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_empleados_accesos_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_empleados_accesos_id_seq OWNER TO admin;

--
-- Name: osiris_empleados_accesos_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_empleados_accesos_id_seq OWNED BY osiris_empleados_accesos.id;


--
-- Name: osiris_empresas; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_empresas (
    descripcion_empresa character varying(80) DEFAULT ''::bpchar,
    direccion_empresa character varying(100) DEFAULT ''::bpchar,
    telefono1_empresa character varying(15) DEFAULT ''::bpchar,
    telefono2_empresa character varying(15) DEFAULT ''::bpchar,
    email_empresa character varying(60) DEFAULT ''::bpchar,
    web_empresa character varying(60) DEFAULT ''::bpchar,
    colonia_empresa character varying(30) DEFAULT ''::bpchar,
    estado_empresa character varying(30) DEFAULT ''::bpchar,
    rfc_cliente_empresa character varying(15) DEFAULT ''::bpchar,
    nombre_asesor_sindical character varying(60) DEFAULT ''::bpchar,
    numero_direccion_empresa character varying(10) DEFAULT ''::bpchar,
    id_empresa integer NOT NULL,
    lista_de_precio boolean DEFAULT false,
    id_tipo_paciente integer DEFAULT 0,
    porcentage_descuento numeric(5,2) DEFAULT 0,
    id_tipo_documento integer DEFAULT 1
);


ALTER TABLE public.osiris_empresas OWNER TO admin;

--
-- Name: TABLE osiris_empresas; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_empresas IS 'Almacena la empresas que tienen convenio con el hospital';


--
-- Name: COLUMN osiris_empresas.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empresas.id_empresa IS 'este campo se enlasa con cobros_enca';


--
-- Name: COLUMN osiris_empresas.lista_de_precio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empresas.lista_de_precio IS 'este campo me indica si este cliente tiene una lista de precio para el enlase de precios';


--
-- Name: COLUMN osiris_empresas.id_tipo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empresas.id_tipo_paciente IS 'determina que tipo de paciente se enlasa con el convenio en la tabla de empresas para lista de precios';


--
-- Name: COLUMN osiris_empresas.porcentage_descuento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empresas.porcentage_descuento IS 'este campo me indica el porcentage de descuento que tiene el cliente';


--
-- Name: COLUMN osiris_empresas.id_tipo_documento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_empresas.id_tipo_documento IS 'este campo se enlasa con la tabla de tipos de documentos';


--
-- Name: osiris_empresas_id_empresa_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_empresas_id_empresa_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_empresas_id_empresa_seq OWNER TO admin;

--
-- Name: osiris_empresas_id_empresa_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_empresas_id_empresa_seq OWNED BY osiris_empresas.id_empresa;


--
-- Name: osiris_erp_abonos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_abonos (
    id_abono integer NOT NULL,
    monto_de_abono_procedimiento numeric(13,5) DEFAULT 0,
    monto_de_abono_factura numeric(13,5) DEFAULT 0,
    numero_recibo_caja integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    numero_factura integer DEFAULT 0,
    fecha_abono date DEFAULT '2008-02-01'::date,
    concepto_del_abono character varying DEFAULT ''::bpchar,
    id_presupuesto integer DEFAULT 0,
    id_paquete integer DEFAULT 0,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    fechahora_registro timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_forma_de_pago integer DEFAULT 1,
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying(15) DEFAULT ''::bpchar,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    pago boolean DEFAULT false,
    honorario_medico boolean DEFAULT false,
    pago_factura boolean DEFAULT false,
    pago_honorario_medico boolean DEFAULT false,
    tipo_comprobante character varying DEFAULT ''::bpchar,
    observaciones character varying DEFAULT ''::bpchar,
    pid_paciente integer,
    id_tipo_comprobante integer DEFAULT 1,
    observaciones2 character varying DEFAULT ''::bpchar,
    observaciones3 character varying DEFAULT ''::bpchar,
    monto_convenio numeric(13,5) DEFAULT 0,
    abono boolean DEFAULT false,
    motivo_eliminacion character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_abonos OWNER TO admin;

--
-- Name: TABLE osiris_erp_abonos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_abonos IS 'Esta tabla almacena todos los abonos a procedimiento y facturas';


--
-- Name: COLUMN osiris_erp_abonos.monto_de_abono_procedimiento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.monto_de_abono_procedimiento IS 'Almacena todos los abonos realizados a un procedimiento';


--
-- Name: COLUMN osiris_erp_abonos.monto_de_abono_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.monto_de_abono_factura IS 'Almacena todo los abonos realizados a las facturas';


--
-- Name: COLUMN osiris_erp_abonos.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.eliminado IS 'bandera que indica si esta eliminado el abono';


--
-- Name: COLUMN osiris_erp_abonos.pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.pago IS 'me indica si el pago es un pago total a un procedimiento';


--
-- Name: COLUMN osiris_erp_abonos.honorario_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.honorario_medico IS 'especifica si el pago es un honorario medico';


--
-- Name: COLUMN osiris_erp_abonos.pago_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.pago_factura IS 'esta bandera indica que el pago que se realizo es de una factura';


--
-- Name: COLUMN osiris_erp_abonos.pago_honorario_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.pago_honorario_medico IS 'bandera que indica si este abono es un honorario medico';


--
-- Name: COLUMN osiris_erp_abonos.tipo_comprobante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.tipo_comprobante IS 'me indica que tipo de comprobante';


--
-- Name: COLUMN osiris_erp_abonos.observaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.observaciones IS 'almacena las observaciones';


--
-- Name: COLUMN osiris_erp_abonos.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.pid_paciente IS 'numero de expediente del paciente';


--
-- Name: COLUMN osiris_erp_abonos.id_tipo_comprobante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.id_tipo_comprobante IS 'este campo se enlasa con la tabla tipo de comprobantes';


--
-- Name: COLUMN osiris_erp_abonos.monto_convenio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.monto_convenio IS 'almacena el valor total del convenio';


--
-- Name: COLUMN osiris_erp_abonos.abono; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.abono IS 'indica si el pago recibido es un abono en caja';


--
-- Name: COLUMN osiris_erp_abonos.motivo_eliminacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_abonos.motivo_eliminacion IS 'almacena el motivo de  la cancelacion del pago';


--
-- Name: osiris_erp_abonos_id_abono_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_abonos_id_abono_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_abonos_id_abono_seq OWNER TO admin;

--
-- Name: osiris_erp_abonos_id_abono_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_abonos_id_abono_seq OWNED BY osiris_erp_abonos.id_abono;


--
-- Name: osiris_erp_clientes; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_clientes (
    id_cliente integer NOT NULL,
    descripcion_cliente character varying DEFAULT ''::bpchar,
    direccion_cliente character varying DEFAULT ''::character varying,
    colonia_cliente character varying DEFAULT ''::character varying,
    municipio_cliente character varying DEFAULT ''::character varying,
    estado_cliente character varying DEFAULT ''::character varying,
    rfc_cliente character varying DEFAULT ''::character varying,
    curp_cliente character varying DEFAULT ''::character varying,
    telefono1_cliente character varying DEFAULT ''::character varying,
    telefono2_cliente character varying DEFAULT ''::character varying,
    fax_cliente character varying DEFAULT ''::character varying,
    mail_cliente character varying DEFAULT ''::character varying,
    contacto_cliente character varying DEFAULT ''::character varying,
    telefono_contacto_cliente character varying DEFAULT ''::character varying,
    fechahora_creacion_cliente timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    historial_cambios text DEFAULT ''::bpchar,
    cp_cliente character varying(10) DEFAULT ''::bpchar,
    fecha_baja_cliente timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_baja character varying(15) DEFAULT ''::bpchar,
    cliente_activo boolean DEFAULT true,
    dias_credito_cliente numeric(3,0) DEFAULT 0,
    celular_cliente character varying DEFAULT ''::character varying,
    id_forma_de_pago integer DEFAULT 1,
    envio_factura boolean DEFAULT false,
    porcentage_descuento numeric(5,2) DEFAULT 0
);


ALTER TABLE public.osiris_erp_clientes OWNER TO admin;

--
-- Name: TABLE osiris_erp_clientes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_clientes IS 'Esta tabla almacena a todos los clientes que requieren factura, esta enlasado con los numeros de atencion (folio de servicio)';


--
-- Name: COLUMN osiris_erp_clientes.cliente_activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_clientes.cliente_activo IS 'me indica si el cliente esta activo';


--
-- Name: COLUMN osiris_erp_clientes.dias_credito_cliente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_clientes.dias_credito_cliente IS 'me indica los dias de credito que tiene el cliente para el vencimiento de factura';


--
-- Name: COLUMN osiris_erp_clientes.id_forma_de_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_clientes.id_forma_de_pago IS 'este campo se enlaza con la tabla erp_forma_de_pago';


--
-- Name: COLUMN osiris_erp_clientes.envio_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_clientes.envio_factura IS 'marca si este cliente tiene convenio con recepcion de facturas';


--
-- Name: COLUMN osiris_erp_clientes.porcentage_descuento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_clientes.porcentage_descuento IS 'este campo me indica el porcentage de descuento que tiene el cliente';


--
-- Name: osiris_erp_clientes_id_cliente_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_clientes_id_cliente_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_clientes_id_cliente_seq OWNER TO admin;

--
-- Name: osiris_erp_clientes_id_cliente_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_clientes_id_cliente_seq OWNED BY osiris_erp_clientes.id_cliente;


--
-- Name: osiris_erp_cobros_deta; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_cobros_deta (
    id_producto numeric(12,0),
    folio_de_servicio integer,
    pid_paciente integer,
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15),
    id_secuencia integer NOT NULL,
    id_tipo_admisiones integer,
    precio_producto numeric(13,5) DEFAULT 0,
    iva_producto numeric(13,5) DEFAULT 0,
    id_tipo_admisiones2 integer DEFAULT 0,
    precio_costo_unitario numeric(13,5) DEFAULT 0,
    porcentage_utilidad numeric(7,3) DEFAULT 0,
    porcentage_descuento numeric(6,3) DEFAULT 0,
    porcentage_iva numeric(5,3) DEFAULT 0,
    precio_costo numeric(13,5) DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_empleado character varying(15) DEFAULT ''::bpchar,
    cantidad_aplicada numeric(13,6) DEFAULT 0,
    id_almacen integer DEFAULT 0,
    precio_por_cantidad numeric(13,2) DEFAULT 0.00,
    numero_factura integer DEFAULT 0,
    folio_interno_dep integer DEFAULT 0,
    fechahora_solicitud timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    historial_traspaso character varying DEFAULT ''::bpchar,
    orden_compra integer,
    id_proveedor integer
);


ALTER TABLE public.osiris_erp_cobros_deta OWNER TO admin;

--
-- Name: TABLE osiris_erp_cobros_deta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_cobros_deta IS 'Almacena el detalle de todos los cargos que se realizan a los pacientes';


--
-- Name: COLUMN osiris_erp_cobros_deta.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.id_producto IS 'este campo esta enlasado con la tabla de productos';


--
-- Name: COLUMN osiris_erp_cobros_deta.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.eliminado IS 'bandera si el producto esta devuelto para que no se cobre al paciente (devolucion)';


--
-- Name: COLUMN osiris_erp_cobros_deta.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.id_tipo_admisiones IS 'especifica el centro de costo del modulo donde se hizo el cargo';


--
-- Name: COLUMN osiris_erp_cobros_deta.precio_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.precio_producto IS 'Este indica el precio a Publico ver campo precio_producto_publico de la tabla productos';


--
-- Name: COLUMN osiris_erp_cobros_deta.id_tipo_admisiones2; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.id_tipo_admisiones2 IS 'este campo determina el centro de costo de donde se realiza el cargo';


--
-- Name: COLUMN osiris_erp_cobros_deta.precio_costo_unitario; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.precio_costo_unitario IS 'Almacena el precio de costo del producto que se cargo a la cuenta';


--
-- Name: COLUMN osiris_erp_cobros_deta.porcentage_utilidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.porcentage_utilidad IS 'Almacena el porcentage de utilidad aplicado al producto';


--
-- Name: COLUMN osiris_erp_cobros_deta.porcentage_descuento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.porcentage_descuento IS 'Almacena el porcentage de descuento aplicado al producto';


--
-- Name: COLUMN osiris_erp_cobros_deta.porcentage_iva; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.porcentage_iva IS 'Almacena el porcentage del Iva';


--
-- Name: COLUMN osiris_erp_cobros_deta.precio_costo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.precio_costo IS 'indica el precio de costo total del producto';


--
-- Name: COLUMN osiris_erp_cobros_deta.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.fechahora_creacion IS 'se almacena la fecha y la hora cuando se realizo este cargo al paciente';


--
-- Name: COLUMN osiris_erp_cobros_deta.id_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.id_empleado IS 'quien realizo cargo';


--
-- Name: COLUMN osiris_erp_cobros_deta.cantidad_aplicada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.cantidad_aplicada IS 'Almacena la cantidad aplicada ';


--
-- Name: COLUMN osiris_erp_cobros_deta.id_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.id_almacen IS 'indical la identificacion del almacen donde se realiza el cargo';


--
-- Name: COLUMN osiris_erp_cobros_deta.numero_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.numero_factura IS 'almacena el numero de factura con el cual se faturo este procedimiento';


--
-- Name: COLUMN osiris_erp_cobros_deta.folio_interno_dep; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.folio_interno_dep IS 'numero de control interno de laboratorio imagenologia';


--
-- Name: COLUMN osiris_erp_cobros_deta.fechahora_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.fechahora_solicitud IS 'alamcena la fecha cuando se realizo la solicitud';


--
-- Name: COLUMN osiris_erp_cobros_deta.historial_traspaso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_deta.historial_traspaso IS 'alamcena el historial de cuando se traspaso el producto';


--
-- Name: osiris_erp_cobros_deta_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_cobros_deta_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_cobros_deta_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_cobros_deta_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_cobros_deta_id_secuencia_seq OWNED BY osiris_erp_cobros_deta.id_secuencia;


--
-- Name: osiris_erp_cobros_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_cobros_enca (
    folio_de_servicio integer DEFAULT 0,
    pid_paciente integer DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_empleado_factura character varying(15) DEFAULT ''::bpchar,
    id_empresa integer DEFAULT 1,
    fecha_facturacion date DEFAULT '2000-01-01'::date,
    numero_factura integer DEFAULT 0,
    numero_ntacred integer DEFAULT 0,
    fecha_reservacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    numero_reservacion integer DEFAULT 0,
    fecha_pago_total date DEFAULT '2000-01-01'::date,
    cerrado boolean DEFAULT false,
    alta_paciente boolean DEFAULT false,
    fecha_alta_paciente timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_empleado_alta_paciente character varying(15) DEFAULT ''::bpchar,
    tiene_abono boolean DEFAULT false,
    fecha_ntacred date DEFAULT '2000-01-01'::date,
    reservacion boolean DEFAULT false,
    facturacion boolean DEFAULT false,
    id_empleado_admision character varying(15),
    responsable_cuenta character varying(70),
    parentezco character varying(15),
    direccion_responsable_cuenta character varying(200),
    telefono1_responsable_cuenta character varying(15),
    empresa_labora_responsable character varying(65),
    paciente_asegurado boolean DEFAULT false,
    id_aseguradora integer DEFAULT 1,
    ocupacion_responsable character varying(40),
    id_medico integer DEFAULT 1,
    refacturacion boolean DEFAULT false,
    numero_refactura integer DEFAULT 0,
    fecha_refacturacion date DEFAULT '2000-01-01'::date,
    id_empleado_refacturacion character varying(15),
    numero_poliza character varying(20) DEFAULT 0,
    direccion_emp_responsable character varying(200) DEFAULT ''::bpchar,
    telefono_emp_responsable character varying(60),
    iva_aplicado numeric(4,2) DEFAULT 0,
    bloqueo_de_folio boolean DEFAULT false,
    historial_de_bloqueo text DEFAULT ''::bpchar,
    cancelado boolean DEFAULT false,
    fechahora_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    motivo_cancelacion character varying(350) DEFAULT ''::bpchar,
    id_quien_cerrocuenta character varying(15),
    pagado boolean DEFAULT false,
    id_quien_cancelo character varying(15),
    nombre_medico_encabezado character varying(60) DEFAULT ''::bpchar,
    id_cliente integer DEFAULT 1,
    valor_de_paquete_quirujico integer DEFAULT 0,
    nombre_empresa_encabezado character varying DEFAULT ''::bpchar,
    honorario_medico numeric(13,5) DEFAULT 0,
    historial_de_cerrado text DEFAULT ''::bpchar,
    deducible numeric(13,5) DEFAULT 0,
    coaseguro numeric(7,2) DEFAULT 0,
    id_cuarto integer DEFAULT 1,
    observaciones_de_alta character varying(300) DEFAULT ''::bpchar,
    motivo_alta_paciente character varying DEFAULT ''::bpchar,
    numero_certificado character varying(20) DEFAULT 0,
    total_procedimiento numeric(13,5) DEFAULT 0,
    subtotal15 numeric(13,2) DEFAULT 0.00,
    subtotal0 numeric(13,2) DEFAULT 0.00,
    id_medico_tratante integer DEFAULT 1,
    fechahora_cerrado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    contador_cerrados integer DEFAULT 0,
    historial_facturados text DEFAULT ''::bpchar,
    nombre_medico_tratante character(70) DEFAULT ''::bpchar,
    total_abonos numeric(15,3) DEFAULT 0,
    total_pago numeric(13,5) DEFAULT 0,
    id_habitacion integer DEFAULT 1 NOT NULL,
    valor_total_notacredito numeric(13,5) DEFAULT 0,
    observacion_ingreso character varying DEFAULT ''::bpchar,
    monto_convenio numeric(13,5) DEFAULT 0,
    historial_convenios text DEFAULT ''::bpchar,
    observaciones1 text DEFAULT ''::bpchar,
    observaciones2 text DEFAULT ''::bpchar,
    historial_comprobantes_caja text DEFAULT ''::bpchar,
    pagare boolean DEFAULT false
);


ALTER TABLE public.osiris_erp_cobros_enca OWNER TO admin;

--
-- Name: TABLE osiris_erp_cobros_enca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_cobros_enca IS 'Almacena los encabezados de cobros a pacientes, movimientos generales hasta alta del mismo';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_empleado_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_empleado_factura IS 'guarda quien realizo la factura';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_empresa IS 'id de la empresa que esta dada de alta en nuestro sistema que tiene convenio comercial';


--
-- Name: COLUMN osiris_erp_cobros_enca.numero_ntacred; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.numero_ntacred IS 'almacena el numero de la nota de credito';


--
-- Name: COLUMN osiris_erp_cobros_enca.fecha_reservacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.fecha_reservacion IS 'aqui grabo la fecha de reservacion si se separa algun paquete';


--
-- Name: COLUMN osiris_erp_cobros_enca.numero_reservacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.numero_reservacion IS 'Almacena el numero de reservacion asignado correlativamente, referencia interna solamente';


--
-- Name: COLUMN osiris_erp_cobros_enca.fecha_pago_total; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.fecha_pago_total IS 'indica la fecha de cuando se pago total esta atencion del paciente';


--
-- Name: COLUMN osiris_erp_cobros_enca.cerrado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.cerrado IS 'indica si este numero de atencion fue pagado completamente';


--
-- Name: COLUMN osiris_erp_cobros_enca.alta_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.alta_paciente IS 'me indica si el paciente se le realizo el alta, ademas bloquea para que no se puedan realizar cargos, solo caja lo podra realizar';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_empleado_alta_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_empleado_alta_paciente IS 'identificacion del usuario quien bloque la cuenta';


--
-- Name: COLUMN osiris_erp_cobros_enca.tiene_abono; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.tiene_abono IS 'me indica si esta atencion tiene algun abono';


--
-- Name: COLUMN osiris_erp_cobros_enca.fecha_ntacred; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.fecha_ntacred IS 'almacena la fecha cuando se realizo la nota de credito';


--
-- Name: COLUMN osiris_erp_cobros_enca.reservacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.reservacion IS 'me indica si esta atencion lleva una reservacion';


--
-- Name: COLUMN osiris_erp_cobros_enca.facturacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.facturacion IS 'me indica si se ha realizado la factura, ademas llena los datos de tacturacion';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_empleado_admision; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_empleado_admision IS 'almacena el id del empleado que creo la admision';


--
-- Name: COLUMN osiris_erp_cobros_enca.responsable_cuenta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.responsable_cuenta IS 'nombre de quien es responsable de la cuenta';


--
-- Name: COLUMN osiris_erp_cobros_enca.parentezco; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.parentezco IS 'parentezco del responsable de la cuenta';


--
-- Name: COLUMN osiris_erp_cobros_enca.paciente_asegurado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.paciente_asegurado IS 'bandera me indica si el paciente entro como asegurado';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_aseguradora; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_aseguradora IS 'codigo de empresa aseguradora';


--
-- Name: COLUMN osiris_erp_cobros_enca.ocupacion_responsable; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.ocupacion_responsable IS 'puesto laboral del reponsable';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_medico IS 'este campo se enlasa con la tabla his_medicos solo es el doctor que toma los datos del archivo clinico';


--
-- Name: COLUMN osiris_erp_cobros_enca.refacturacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.refacturacion IS 'me indica si esta refacturado';


--
-- Name: COLUMN osiris_erp_cobros_enca.numero_refactura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.numero_refactura IS 'numero de refactura';


--
-- Name: COLUMN osiris_erp_cobros_enca.numero_poliza; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.numero_poliza IS 'numero de poliza dependiendo la compañia de seguros';


--
-- Name: COLUMN osiris_erp_cobros_enca.telefono_emp_responsable; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.telefono_emp_responsable IS 'Telefono de la empresa donde labora el responsable del paciente';


--
-- Name: COLUMN osiris_erp_cobros_enca.bloqueo_de_folio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.bloqueo_de_folio IS 'si esta activo el folio se bloquea y no se pueden agregar nuevos movimientos';


--
-- Name: COLUMN osiris_erp_cobros_enca.cancelado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.cancelado IS 'indica si el folio esta cancelado, solo se cancela los que no tienen movimiento en tabla detalle';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_quien_cerrocuenta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_quien_cerrocuenta IS 'Almacena quien cerro la cuenta del paciente';


--
-- Name: COLUMN osiris_erp_cobros_enca.pagado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.pagado IS 'indica que este procedimiento fue pagado completamente en el proceso de caja';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_quien_cancelo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_quien_cancelo IS 'identifica quien cancelo este folio';


--
-- Name: COLUMN osiris_erp_cobros_enca.nombre_medico_encabezado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.nombre_medico_encabezado IS 'nombre de medico que se toma cuando no existe medico en la base de datos';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_cliente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_cliente IS 'identifica al clientes de la facturacion se enlasa con tabla erp_clientes';


--
-- Name: COLUMN osiris_erp_cobros_enca.valor_de_paquete_quirujico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.valor_de_paquete_quirujico IS 'es valor total del paquete se ha separado por el paciente';


--
-- Name: COLUMN osiris_erp_cobros_enca.honorario_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.honorario_medico IS 'Almacena el honorario que se le pago al doctor';


--
-- Name: COLUMN osiris_erp_cobros_enca.historial_de_cerrado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.historial_de_cerrado IS 'Almacena el historial de cuando se cerro ese folio';


--
-- Name: COLUMN osiris_erp_cobros_enca.deducible; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.deducible IS 'almacena el valor total del deducible';


--
-- Name: COLUMN osiris_erp_cobros_enca.coaseguro; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.coaseguro IS 'almacena el porcentage del coaseguro';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_cuarto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_cuarto IS 'es el numero de cuarto o cubiculo asignado al paciente, enlasada a la tabla his_cuartos';


--
-- Name: COLUMN osiris_erp_cobros_enca.observaciones_de_alta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.observaciones_de_alta IS 'observaciones realizadas por la enfermera que dio de alta';


--
-- Name: COLUMN osiris_erp_cobros_enca.motivo_alta_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.motivo_alta_paciente IS 'motivo por el que se dio de alta al paciente';


--
-- Name: COLUMN osiris_erp_cobros_enca.numero_certificado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.numero_certificado IS 'numero de certificado de seguro colectivo';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_medico_tratante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_medico_tratante IS 'este campo almacen al medico tratante se enlasa con his_medicos';


--
-- Name: COLUMN osiris_erp_cobros_enca.fechahora_cerrado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.fechahora_cerrado IS 'almacena la fecha y hora cuando se realizo el ultimo cierre';


--
-- Name: COLUMN osiris_erp_cobros_enca.contador_cerrados; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.contador_cerrados IS 'almacena el numero de veces que se ha cerrado';


--
-- Name: COLUMN osiris_erp_cobros_enca.historial_facturados; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.historial_facturados IS 'almacena el historial de facturacion de los procedimiento en caso de que se cancele la factura';


--
-- Name: COLUMN osiris_erp_cobros_enca.nombre_medico_tratante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.nombre_medico_tratante IS 'nombre del medico tratante';


--
-- Name: COLUMN osiris_erp_cobros_enca.total_abonos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.total_abonos IS 'Almacena todos los abonos que se realizan al paceinte';


--
-- Name: COLUMN osiris_erp_cobros_enca.total_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.total_pago IS 'almacena el total de los pagos';


--
-- Name: COLUMN osiris_erp_cobros_enca.id_habitacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.id_habitacion IS 'id habitacion, 1 = no tine habitacion ';


--
-- Name: COLUMN osiris_erp_cobros_enca.valor_total_notacredito; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.valor_total_notacredito IS 'almacena el valor total de la nota de credito para restarla en le procedimiento';


--
-- Name: COLUMN osiris_erp_cobros_enca.observacion_ingreso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.observacion_ingreso IS 'observacion de ingreso';


--
-- Name: COLUMN osiris_erp_cobros_enca.monto_convenio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.monto_convenio IS 'almacena el valor total del convenio que hizo con el hospital';


--
-- Name: COLUMN osiris_erp_cobros_enca.historial_convenios; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.historial_convenios IS 'almacena los movimiento de los convenios realizados';


--
-- Name: COLUMN osiris_erp_cobros_enca.historial_comprobantes_caja; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_cobros_enca.historial_comprobantes_caja IS 'almacena el historial de los comprobantes de caja pagos y/o abono a procedimiento';


--
-- Name: osiris_erp_codigos_barra; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_codigos_barra (
    id_secuencia integer NOT NULL,
    id_codigo_barras character varying DEFAULT ''::bpchar,
    id_producto numeric(12,0),
    marca character varying DEFAULT ''::bpchar,
    id_quien_creo character varying DEFAULT ''::bpchar,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying DEFAULT ''::bpchar,
    presentacion character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_codigos_barra OWNER TO admin;

--
-- Name: osiris_erp_codigos_barra_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_codigos_barra_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_codigos_barra_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_codigos_barra_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_codigos_barra_id_secuencia_seq OWNED BY osiris_erp_codigos_barra.id_secuencia;


--
-- Name: osiris_erp_compra_farmacia; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_compra_farmacia (
    id_secuencia integer NOT NULL,
    folio_de_servicio integer,
    orden_compra integer,
    id_producto numeric(12,0),
    id_subalmacen integer,
    cantidad_embalaje numeric(13,5) DEFAULT 0,
    cantidad_autorizo numeric(13,5) DEFAULT 0,
    total_surtir numeric(13,5) DEFAULT 0,
    fechahora_autorizacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_compro character varying(15),
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_proveedor integer,
    id_medico integer DEFAULT 1,
    costo_por_unidad numeric(13,5) DEFAULT 0,
    precio_producto_publico numeric(13,5) DEFAULT 0,
    porcentage_ganancia numeric(7,3) DEFAULT 0,
    costo_producto numeric(13,5) DEFAULT 0
);


ALTER TABLE public.osiris_erp_compra_farmacia OWNER TO admin;

--
-- Name: TABLE osiris_erp_compra_farmacia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_compra_farmacia IS 'guarda las compras que se realizan a farmacia';


--
-- Name: COLUMN osiris_erp_compra_farmacia.id_secuencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.id_secuencia IS 'secuencia de numeros';


--
-- Name: COLUMN osiris_erp_compra_farmacia.folio_de_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.folio_de_servicio IS 'es el folio del servicio del procedimiento';


--
-- Name: COLUMN osiris_erp_compra_farmacia.orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.orden_compra IS 'almacena el numero de la orden originado por farmacia';


--
-- Name: COLUMN osiris_erp_compra_farmacia.id_subalmacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.id_subalmacen IS 'es el id del almacen donde se cargo';


--
-- Name: COLUMN osiris_erp_compra_farmacia.cantidad_embalaje; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.cantidad_embalaje IS 'es la cantidad que trae el paquete';


--
-- Name: COLUMN osiris_erp_compra_farmacia.cantidad_autorizo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.cantidad_autorizo IS 'cantidad autorizada de compra';


--
-- Name: COLUMN osiris_erp_compra_farmacia.total_surtir; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.total_surtir IS 'almacena la cantidad neta que fue surtida de esa orden';


--
-- Name: COLUMN osiris_erp_compra_farmacia.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.eliminado IS 'fecha en que fue eliminada la orden';


--
-- Name: COLUMN osiris_erp_compra_farmacia.costo_por_unidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.costo_por_unidad IS 'almacena el costo unitario';


--
-- Name: COLUMN osiris_erp_compra_farmacia.precio_producto_publico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.precio_producto_publico IS 'es el precio que se le da al publico en general';


--
-- Name: COLUMN osiris_erp_compra_farmacia.costo_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_compra_farmacia.costo_producto IS 'Es eL costo de producto';


--
-- Name: osiris_erp_compra_farmacia_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_compra_farmacia_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_compra_farmacia_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_compra_farmacia_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_compra_farmacia_id_secuencia_seq OWNED BY osiris_erp_compra_farmacia.id_secuencia;


--
-- Name: osiris_erp_comprobante_pagare; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_comprobante_pagare (
    id_comprobante_pagare integer NOT NULL,
    numero_comprobante_pagare integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    pid_paciente integer DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    numero_factura integer DEFAULT 0,
    fecha_comprobante date DEFAULT '2000-01-01'::date,
    concepto_del_comprobante character varying DEFAULT ''::bpchar,
    id_presupuesto integer DEFAULT 1,
    id_paquete integer DEFAULT 1,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    id_empresa integer DEFAULT 1,
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying(14) DEFAULT ''::bpchar,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    observaciones character varying DEFAULT ''::bpchar,
    id_tipo_paciente integer DEFAULT 0,
    observaciones2 character varying DEFAULT ''::bpchar,
    observaciones3 character varying DEFAULT ''::bpchar,
    monto_pagare numeric(13,5),
    fecha_vencimiento_pagare date DEFAULT '2000-01-01'::date,
    monto_convenio numeric(13,5) DEFAULT 0,
    id_tipo_comprobante integer DEFAULT 1,
    id_tipo_admisiones integer DEFAULT 0,
    pagare boolean DEFAULT false,
    motivo_eliminacion character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_comprobante_pagare OWNER TO admin;

--
-- Name: TABLE osiris_erp_comprobante_pagare; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_comprobante_pagare IS 'Esta tabla almacena los comprobantes de pagares que se realizan';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.numero_comprobante_pagare; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.numero_comprobante_pagare IS 'almacena el numero del comprobante de pagare';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.fecha_comprobante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.fecha_comprobante IS 'almacena la fecha de cuando se realizo el comprobante de pagare';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.concepto_del_comprobante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.concepto_del_comprobante IS 'almacen el concepto de porque se realizo el comprobante de pagare';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.id_presupuesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.id_presupuesto IS 'se enlasa con la tabla de presupuesto';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.id_paquete; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.id_paquete IS 'se enlasa con la tabla de paquetes y cirugias';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.id_quien_creo IS 'alamacena el login de quien realizo esta transaccion';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.id_empresa IS 'este campo se enlasa con la tabla de empresas';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.observaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.observaciones IS 'almacena la observacion del comprobante';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.id_tipo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.id_tipo_paciente IS 'este campo se enlasa a la tabla de tipos de pacientes';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.monto_convenio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.monto_convenio IS 'almacena el valor total del convenio del procedimiento';


--
-- Name: COLUMN osiris_erp_comprobante_pagare.pagare; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_pagare.pagare IS 'bandera que indica si el procedimiento tiene ingresado un pagare';


--
-- Name: osiris_erp_comprobante_pagare_id_comprobante_pagare_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_comprobante_pagare_id_comprobante_pagare_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_comprobante_pagare_id_comprobante_pagare_seq OWNER TO admin;

--
-- Name: osiris_erp_comprobante_pagare_id_comprobante_pagare_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_comprobante_pagare_id_comprobante_pagare_seq OWNED BY osiris_erp_comprobante_pagare.id_comprobante_pagare;


--
-- Name: osiris_erp_comprobante_servicio; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_comprobante_servicio (
    id_comprobante_servicio integer NOT NULL,
    numero_comprobante_servicio integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    pid_paciente integer DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    numero_factura integer DEFAULT 0,
    fecha_comprobante date DEFAULT '2000-01-01'::date,
    concepto_del_comprobante character varying DEFAULT ''::bpchar,
    id_presupuesto integer DEFAULT 1,
    id_paquete integer DEFAULT 1,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    id_empresa integer DEFAULT 1,
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying(14) DEFAULT ''::bpchar,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    observaciones character varying DEFAULT ''::bpchar,
    id_tipo_paciente integer DEFAULT 0,
    observaciones2 character varying DEFAULT ''::bpchar,
    observaciones3 character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_comprobante_servicio OWNER TO admin;

--
-- Name: TABLE osiris_erp_comprobante_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_comprobante_servicio IS 'Esta tabla almacena los comprobantes de servicios que se realizan';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.numero_comprobante_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.numero_comprobante_servicio IS 'almacena el numero del comprobante de servicio';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.fecha_comprobante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.fecha_comprobante IS 'almacena la fecha de cuando se realizo el comprobante de servicio';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.concepto_del_comprobante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.concepto_del_comprobante IS 'almacen el concepto de porque se realizo el comprobante de servicio';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.id_presupuesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.id_presupuesto IS 'se enlasa con la tabla de presupuesto';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.id_paquete; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.id_paquete IS 'se enlasa con la tabla de paquetes y cirugias';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.id_quien_creo IS 'alamacena el login de quien realizo esta transaccion';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.id_empresa IS 'este campo se enlasa con la tabla de empresas';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.observaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.observaciones IS 'almacena la observacion del comprobante';


--
-- Name: COLUMN osiris_erp_comprobante_servicio.id_tipo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_comprobante_servicio.id_tipo_paciente IS 'este campo se enlasa a la tabla de tipos de pacientes';


--
-- Name: osiris_erp_comprobante_servicio_id_comprobante_servicio_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_comprobante_servicio_id_comprobante_servicio_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_comprobante_servicio_id_comprobante_servicio_seq OWNER TO admin;

--
-- Name: osiris_erp_comprobante_servicio_id_comprobante_servicio_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_comprobante_servicio_id_comprobante_servicio_seq OWNED BY osiris_erp_comprobante_servicio.id_comprobante_servicio;


--
-- Name: osiris_erp_convenios; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_convenios (
    id_convenio integer NOT NULL,
    descripcion_convenio character varying
);


ALTER TABLE public.osiris_erp_convenios OWNER TO admin;

--
-- Name: osiris_erp_convenios_id_convenio_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_convenios_id_convenio_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_convenios_id_convenio_seq OWNER TO admin;

--
-- Name: osiris_erp_convenios_id_convenio_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_convenios_id_convenio_seq OWNED BY osiris_erp_convenios.id_convenio;


--
-- Name: osiris_erp_documentos_convenio; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_documentos_convenio (
    id_empresa integer DEFAULT 1,
    id_aseguradora integer DEFAULT 1,
    descripcion_documento character varying DEFAULT ''::bpchar,
    activo boolean DEFAULT true,
    id_secuencia integer NOT NULL,
    id_tipo_documento integer DEFAULT 1,
    id_tipo_paciente integer DEFAULT 0
);


ALTER TABLE public.osiris_erp_documentos_convenio OWNER TO admin;

--
-- Name: TABLE osiris_erp_documentos_convenio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_documentos_convenio IS 'Tabla que indica que documentos debe llenar al ingresar un paciente';


--
-- Name: COLUMN osiris_erp_documentos_convenio.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_documentos_convenio.id_empresa IS 'este campo se enlasa con la tabla de empresas';


--
-- Name: COLUMN osiris_erp_documentos_convenio.id_aseguradora; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_documentos_convenio.id_aseguradora IS 'este campo se enlasa con la tabla de aseguradoras';


--
-- Name: COLUMN osiris_erp_documentos_convenio.id_tipo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_documentos_convenio.id_tipo_paciente IS 'este campo se enlasa con la tabla tipo de pacientes';


--
-- Name: osiris_erp_documentos_convenio_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_documentos_convenio_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_documentos_convenio_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_documentos_convenio_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_documentos_convenio_id_secuencia_seq OWNED BY osiris_erp_documentos_convenio.id_secuencia;


--
-- Name: osiris_erp_emisor; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_emisor (
    emisor character varying DEFAULT ''::bpchar,
    rfc character varying DEFAULT ''::bpchar,
    calle character varying DEFAULT ''::bpchar,
    noexterior character varying DEFAULT ''::bpchar,
    nointerior character varying DEFAULT ''::bpchar,
    colonia character varying DEFAULT ''::bpchar,
    localidad character varying DEFAULT ''::bpchar,
    referencia character varying DEFAULT ''::bpchar,
    municipio character varying DEFAULT ''::bpchar,
    estado character varying DEFAULT ''::bpchar,
    pais character varying DEFAULT ''::bpchar,
    codigopostal character varying(10) DEFAULT ''::bpchar,
    id_emisor integer NOT NULL,
    emisorlocation_logo character varying DEFAULT ''::bpchar,
    emisorlocation_rfc character varying DEFAULT ''::bpchar,
    sigla_emisor character varying DEFAULT ''::bpchar,
    activo boolean DEFAULT true,
    emisorlocation_p12 character varying DEFAULT ''::bpchar,
    passwd_keyp12 character varying DEFAULT ''::bpchar,
    name_file_cer character varying DEFAULT ''::bpchar,
    genera_cfd boolean DEFAULT false,
    locationfile_cfd character varying DEFAULT ''::bpchar,
    locationfile_xslt character varying DEFAULT ''::bpchar,
    id_certificado integer DEFAULT 1,
    telefono character varying DEFAULT ''::bpchar,
    email character varying DEFAULT ''::bpchar,
    id_almacen integer DEFAULT 1,
    regimen character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_emisor OWNER TO admin;

--
-- Name: TABLE osiris_erp_emisor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_emisor IS 'Tabla que almacena los emisores de facturas electronicas';


--
-- Name: COLUMN osiris_erp_emisor.emisorlocation_logo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.emisorlocation_logo IS 'ubicacion donde se recogera el logo de la empresa';


--
-- Name: COLUMN osiris_erp_emisor.emisorlocation_rfc; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.emisorlocation_rfc IS 'ubicacion del rfc del emisor de la factura';


--
-- Name: COLUMN osiris_erp_emisor.sigla_emisor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.sigla_emisor IS 'aqui se pone la sigla del emisor de la factura';


--
-- Name: COLUMN osiris_erp_emisor.activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.activo IS 'activa y desactiva emisor de facturas';


--
-- Name: COLUMN osiris_erp_emisor.emisorlocation_p12; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.emisorlocation_p12 IS 'ubicacion del archivo p12 con el certificado y la llave';


--
-- Name: COLUMN osiris_erp_emisor.passwd_keyp12; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.passwd_keyp12 IS 'passwd de la llave';


--
-- Name: COLUMN osiris_erp_emisor.name_file_cer; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.name_file_cer IS 'poner en nombre del archivo p12 creado';


--
-- Name: COLUMN osiris_erp_emisor.genera_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.genera_cfd IS 'autorizacion para crear el comprobante fiscal digital';


--
-- Name: COLUMN osiris_erp_emisor.locationfile_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.locationfile_cfd IS 'ubicacion donde se dejaran los comprobantes fiscales digitales';


--
-- Name: COLUMN osiris_erp_emisor.locationfile_xslt; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.locationfile_xslt IS 'ubicaion de los archivos xslt que crean la cadena original segun SAT de Mexico';


--
-- Name: COLUMN osiris_erp_emisor.telefono; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.telefono IS 'telefono del emisor';


--
-- Name: COLUMN osiris_erp_emisor.email; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.email IS 'correo electronico del emisor';


--
-- Name: COLUMN osiris_erp_emisor.id_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.id_almacen IS 'se enlasa con el almacen general que tiene este emisor';


--
-- Name: COLUMN osiris_erp_emisor.regimen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_emisor.regimen IS 'regimen indicado por el SAT';


--
-- Name: osiris_erp_emisor_id_emisor_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_emisor_id_emisor_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_emisor_id_emisor_seq OWNER TO admin;

--
-- Name: osiris_erp_emisor_id_emisor_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_emisor_id_emisor_seq OWNED BY osiris_erp_emisor.id_emisor;


--
-- Name: osiris_erp_factura_compra_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_factura_compra_enca (
    numero_orden_compra integer DEFAULT 0,
    fechahora_orden_compra timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    cancelado boolean DEFAULT false,
    fechahora_cancelado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_proveedor integer DEFAULT 1,
    factura_sin_orden_compra boolean DEFAULT false,
    numero_factura_proveedor character varying DEFAULT ''::character varying,
    id_quien_cancelo character varying(15) DEFAULT ''::bpchar,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fecha_factura date DEFAULT '2000-01-01'::date,
    numero_serie_cfd character varying DEFAULT ''::bpchar,
    ano_aprobacion_cfd character varying DEFAULT ''::bpchar,
    numero_aprobacion_cfd character varying DEFAULT ''::bpchar,
    numero_requisicion integer DEFAULT 0,
    subtotal_factura numeric(13,5) DEFAULT 0,
    iva_factura numeric(13,5) DEFAULT 0.00,
    total_factura numeric(13,5) DEFAULT 0,
    valida_conta boolean DEFAULT false,
    fechahora_valida_conta timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_validaconta character varying DEFAULT ''::bpchar,
    remisionada boolean DEFAULT false,
    fechahora_remision timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_remisiono character varying DEFAULT ''::bpchar,
    numero_remision integer DEFAULT 0,
    id_secuencia integer NOT NULL,
    fecha_entrada_almacen date DEFAULT '2000-01-01'::date,
    id_emisor integer DEFAULT 1
);


ALTER TABLE public.osiris_erp_factura_compra_enca OWNER TO admin;

--
-- Name: TABLE osiris_erp_factura_compra_enca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_factura_compra_enca IS 'Guarda informacion sobre las facturas de ordenes de compras';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.numero_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.numero_orden_compra IS 'este campo se enlasa con la tabla de ordenes de compra enca';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.factura_sin_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.factura_sin_orden_compra IS 'me indica si la factura ingresada es con o sin orden de compra';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.numero_serie_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.numero_serie_cfd IS 'almacena el numero de serie de la factura';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.ano_aprobacion_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.ano_aprobacion_cfd IS 'almacena el año de aprobacion de del cfd';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.numero_aprobacion_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.numero_aprobacion_cfd IS 'almacena el numero de aprobacion del cfd';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.valida_conta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.valida_conta IS 'bandera que indica si la factura fue validada por contabilidad';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.fechahora_valida_conta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.fechahora_valida_conta IS 'fecha y hora cuando valido contabilidad';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.remisionada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.remisionada IS 'bandera que indica s esta remisionada la factura del proveedor';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.fecha_entrada_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.fecha_entrada_almacen IS 'almacen la fecha cuando el proveer llegar a dejar la mercancia en almacen';


--
-- Name: COLUMN osiris_erp_factura_compra_enca.id_emisor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_compra_enca.id_emisor IS 'este campo se enlasa con la tabla de emisores para CFD';


--
-- Name: osiris_erp_factura_compra_enca_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_factura_compra_enca_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_factura_compra_enca_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_factura_compra_enca_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_factura_compra_enca_id_secuencia_seq OWNED BY osiris_erp_factura_compra_enca.id_secuencia;


--
-- Name: osiris_erp_factura_deta; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_factura_deta (
    numero_factura integer,
    cantidad_detalle numeric(9,2),
    descripcion_detalle character varying(100),
    precio_unitario numeric(13,5),
    importe_detalle numeric(13,5),
    id_secuencia integer DEFAULT nextval(('public.osiris_secuencia_deta_factura_seq'::text)::regclass),
    tipo_detalle boolean DEFAULT false,
    detalle_contable boolean DEFAULT false
);


ALTER TABLE public.osiris_erp_factura_deta OWNER TO admin;

--
-- Name: TABLE osiris_erp_factura_deta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_factura_deta IS 'Almacena el detalle de todas las facturas realizadas';


--
-- Name: COLUMN osiris_erp_factura_deta.cantidad_detalle; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_deta.cantidad_detalle IS 'indica la cantidad del detalle';


--
-- Name: COLUMN osiris_erp_factura_deta.descripcion_detalle; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_deta.descripcion_detalle IS 'indica la descripcion del detalle';


--
-- Name: COLUMN osiris_erp_factura_deta.tipo_detalle; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_deta.tipo_detalle IS 'me indica que tipo de detalle es para poder imprimir al final de la hoja';


--
-- Name: COLUMN osiris_erp_factura_deta.detalle_contable; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_deta.detalle_contable IS 'me indica si este detalle es con formula matematica';


--
-- Name: osiris_erp_factura_deta_prov; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_factura_deta_prov (
    id_secuencia integer NOT NULL,
    id_proveedor integer DEFAULT 1
);


ALTER TABLE public.osiris_erp_factura_deta_prov OWNER TO admin;

--
-- Name: COLUMN osiris_erp_factura_deta_prov.id_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_deta_prov.id_proveedor IS 'este campo se enlaza con la tabla de proveedores';


--
-- Name: osiris_erp_factura_deta_prov_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_factura_deta_prov_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_factura_deta_prov_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_factura_deta_prov_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_factura_deta_prov_id_secuencia_seq OWNED BY osiris_erp_factura_deta_prov.id_secuencia;


--
-- Name: osiris_erp_factura_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_factura_enca (
    id_cliente integer NOT NULL,
    descripcion_cliente character varying DEFAULT ''::bpchar,
    direccion_cliente character varying DEFAULT ''::character varying,
    colonia_cliente character varying DEFAULT ''::character varying,
    municipio_cliente character varying DEFAULT ''::character varying,
    estado_cliente character varying DEFAULT ''::character varying,
    rfc_cliente character varying DEFAULT ''::character varying,
    curp_cliente character varying DEFAULT ''::character varying,
    telefono1_cliente character varying DEFAULT ''::character varying,
    telefono2_cliente character varying DEFAULT ''::character varying,
    fax_cliente character varying DEFAULT ''::character varying,
    mail_cliente character varying DEFAULT ''::character varying,
    contacto_cliente character varying DEFAULT ''::character varying,
    telefono_contacto_cliente character varying DEFAULT ''::character varying,
    id_quien_creo character varying(15) DEFAULT ''::bpchar NOT NULL,
    deducible numeric(13,5) DEFAULT 0,
    coaseguro numeric(13,3) DEFAULT 0,
    honorario_medico numeric(13,5) DEFAULT 0,
    fechahora_creacion_factura timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    dias_credito_cliente numeric(3,0) DEFAULT 0,
    cp_cliente character varying(8) DEFAULT ''::bpchar,
    sub_total_15 numeric(13,5) DEFAULT 0,
    sub_total_0 numeric(13,5) DEFAULT 0,
    iva_al_15 numeric(13,5) DEFAULT 0,
    valor_coaseguro numeric(13,5) DEFAULT 0,
    numero_factura integer,
    cancelado boolean DEFAULT false,
    fechahora_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    motivo_cancelacion character varying DEFAULT ''::bpchar,
    fechahora_pago_factura timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_pago character varying(15),
    pagada boolean DEFAULT false,
    fecha_factura date DEFAULT '2000-01-01'::date,
    id_quien_cancelo character varying(15) DEFAULT ''::bpchar,
    total_abonos numeric(15,3) DEFAULT 0,
    enviado boolean DEFAULT false,
    fecha_de_envio timestamp without time zone DEFAULT '2000-01-01 00:00:00'::timestamp without time zone NOT NULL,
    numero_ntacred integer DEFAULT 0,
    fechahora_creacion_ntacred timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo_ntacred character varying(15) DEFAULT ''::bpchar,
    total_ntacred numeric(13,5) DEFAULT 0
);


ALTER TABLE public.osiris_erp_factura_enca OWNER TO admin;

--
-- Name: COLUMN osiris_erp_factura_enca.deducible; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.deducible IS 'almacena el valor total del deducible';


--
-- Name: COLUMN osiris_erp_factura_enca.coaseguro; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.coaseguro IS 'almacena el valor de coaseguro';


--
-- Name: COLUMN osiris_erp_factura_enca.valor_coaseguro; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.valor_coaseguro IS 'almacena el valor total del coaseguro';


--
-- Name: COLUMN osiris_erp_factura_enca.numero_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.numero_factura IS 'almacena el numero factura';


--
-- Name: COLUMN osiris_erp_factura_enca.cancelado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.cancelado IS 'indica si la factura ha sido cancelada';


--
-- Name: COLUMN osiris_erp_factura_enca.fechahora_pago_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.fechahora_pago_factura IS 'almacena la fecha y hora cuando se realizo el pago en el sistema';


--
-- Name: COLUMN osiris_erp_factura_enca.fecha_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.fecha_factura IS 'almacena la fecha de facturacion impresa';


--
-- Name: COLUMN osiris_erp_factura_enca.id_quien_cancelo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.id_quien_cancelo IS 'quien cancela factura';


--
-- Name: COLUMN osiris_erp_factura_enca.enviado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.enviado IS 'true = enviado  , false = no enviado';


--
-- Name: COLUMN osiris_erp_factura_enca.numero_ntacred; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.numero_ntacred IS 'almacena el numerod de la nota de credito';


--
-- Name: COLUMN osiris_erp_factura_enca.fechahora_creacion_ntacred; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_factura_enca.fechahora_creacion_ntacred IS 'fecha de cuando se realiza la nota de credito';


--
-- Name: osiris_erp_forma_de_pago; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_forma_de_pago (
    id_forma_de_pago integer NOT NULL,
    descripcion_forma_de_pago character varying(60) DEFAULT ''::bpchar,
    dias_credito integer DEFAULT 0,
    proveedor boolean
);


ALTER TABLE public.osiris_erp_forma_de_pago OWNER TO admin;

--
-- Name: TABLE osiris_erp_forma_de_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_forma_de_pago IS 'almacena las formas de pago a los proveedores';


--
-- Name: COLUMN osiris_erp_forma_de_pago.id_forma_de_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_forma_de_pago.id_forma_de_pago IS 'consecutivo de forma de pago';


--
-- Name: COLUMN osiris_erp_forma_de_pago.descripcion_forma_de_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_forma_de_pago.descripcion_forma_de_pago IS 'descripcion de la forma de pago';


--
-- Name: COLUMN osiris_erp_forma_de_pago.dias_credito; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_forma_de_pago.dias_credito IS 'dias de credito';


--
-- Name: osiris_erp_forma_de_pago_id_forma_de_pago_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_forma_de_pago_id_forma_de_pago_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_forma_de_pago_id_forma_de_pago_seq OWNER TO admin;

--
-- Name: osiris_erp_forma_de_pago_id_forma_de_pago_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_forma_de_pago_id_forma_de_pago_seq OWNED BY osiris_erp_forma_de_pago.id_forma_de_pago;


--
-- Name: osiris_erp_honorarios_medicos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_honorarios_medicos (
    id_abono integer NOT NULL,
    id_medico integer DEFAULT 1,
    folio_de_servicio integer DEFAULT 0,
    fechahora_abono timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    numero_factura integer DEFAULT 0,
    monto_del_abono numeric DEFAULT 0,
    id_quien_abono character varying,
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15) DEFAULT ''::bpchar,
    pagado boolean DEFAULT false,
    fechahora_pagado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_pago character varying(15) DEFAULT ''::bpchar,
    id_forma_pago integer DEFAULT 0,
    fecha_pago date DEFAULT '2000-01-01'::date
);


ALTER TABLE public.osiris_erp_honorarios_medicos OWNER TO admin;

--
-- Name: TABLE osiris_erp_honorarios_medicos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_honorarios_medicos IS 'Aqui se alamacena el honorario medico, que se otorgan en cada procedimiento';


--
-- Name: COLUMN osiris_erp_honorarios_medicos.id_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_honorarios_medicos.id_medico IS 'este campo se enlasa con la tabla de doctores';


--
-- Name: COLUMN osiris_erp_honorarios_medicos.monto_del_abono; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_honorarios_medicos.monto_del_abono IS 'costo de honorario';


--
-- Name: COLUMN osiris_erp_honorarios_medicos.id_quien_abono; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_honorarios_medicos.id_quien_abono IS 'empleado que realizo el abono de honorarios';


--
-- Name: COLUMN osiris_erp_honorarios_medicos.fechahora_pagado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_honorarios_medicos.fechahora_pagado IS 'almacena la fecha y hora de operacion de cuando se realizo el pago';


--
-- Name: COLUMN osiris_erp_honorarios_medicos.fecha_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_honorarios_medicos.fecha_pago IS 'almacena la fecha de pago ';


--
-- Name: osiris_erp_honorarios_medicos_id_abono_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_honorarios_medicos_id_abono_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_honorarios_medicos_id_abono_seq OWNER TO admin;

--
-- Name: osiris_erp_honorarios_medicos_id_abono_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_honorarios_medicos_id_abono_seq OWNED BY osiris_erp_honorarios_medicos.id_abono;


--
-- Name: osiris_erp_movcargos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_movcargos (
    id_tipo_admisiones integer,
    id_empleado character varying(15),
    folio_de_servicio integer NOT NULL,
    folio_de_servicio_dep integer NOT NULL,
    pid_paciente integer NOT NULL,
    id_movcargos integer NOT NULL,
    id_tipo_paciente integer,
    fechahora_admision_registro timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    numero_factura integer DEFAULT 0,
    id_tipo_cirugia integer DEFAULT 1,
    id_diagnostico integer DEFAULT 1,
    descripcion_diagnostico_movcargos text DEFAULT ''::bpchar,
    lugar_de_ingreso integer DEFAULT 1,
    id_habitacion_cubiculo integer DEFAULT 1,
    vigente boolean DEFAULT true,
    nombre_de_cirugia character varying(250) DEFAULT ''::bpchar,
    id_cie_10 integer DEFAULT 1,
    id_diagnostico_final integer DEFAULT 1,
    descripcion_diagnostico_cie10 character varying DEFAULT ''::bpchar,
    descripcion_diagnostico_final character varying DEFAULT ''::bpchar,
    tipo_cirugia character varying DEFAULT ''::bpchar,
    diagnostico_primeravez character varying(2) DEFAULT ''::bpchar,
    id_producto numeric(12,0) DEFAULT 0,
    id_anestesiologo integer DEFAULT 1,
    producto_observacion1 character varying DEFAULT ''::bpchar,
    producto_observacion2 character varying DEFAULT ''::bpchar,
    id_tecnico_vision character varying DEFAULT ''::bpchar,
    id_medico_asistente integer DEFAULT 1,
    tipo_anestesia character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_movcargos OWNER TO admin;

--
-- Name: TABLE osiris_erp_movcargos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_movcargos IS 'Esta tabla me indica el movimiento de los lugares donde se hacen los cargos que le han realizado al paciente, ej: quirofano, hospital, urgencia, etc.';


--
-- Name: COLUMN osiris_erp_movcargos.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_tipo_admisiones IS 'Este campo esta enlasado a la tabla his_tipo_admisiones';


--
-- Name: COLUMN osiris_erp_movcargos.id_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_empleado IS 'aqui se almacena quien relizo esta solicitud';


--
-- Name: COLUMN osiris_erp_movcargos.folio_de_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.folio_de_servicio IS 'Indica el folio de atencion de paciente al hospital';


--
-- Name: COLUMN osiris_erp_movcargos.folio_de_servicio_dep; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.folio_de_servicio_dep IS 'Folio interno del departamento que relizo el servicio';


--
-- Name: COLUMN osiris_erp_movcargos.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.pid_paciente IS 'es el numero del expediente clinico del paciente';


--
-- Name: COLUMN osiris_erp_movcargos.id_movcargos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_movcargos IS 'correlativo de creacion';


--
-- Name: COLUMN osiris_erp_movcargos.id_tipo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_tipo_paciente IS 'Este campo esta enlasado con la tabla his_tipo_paciente';


--
-- Name: COLUMN osiris_erp_movcargos.fechahora_admision_registro; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.fechahora_admision_registro IS 'graba la fecha y la hora de las banderas de cambios';


--
-- Name: COLUMN osiris_erp_movcargos.numero_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.numero_factura IS 'almacena el numero de la factura por este servicio';


--
-- Name: COLUMN osiris_erp_movcargos.id_tipo_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_tipo_cirugia IS 'Campo enlasado con la tabla his_tipo_cirugias';


--
-- Name: COLUMN osiris_erp_movcargos.id_diagnostico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_diagnostico IS 'este campo se enlasa con la tabla de diagnosticos';


--
-- Name: COLUMN osiris_erp_movcargos.descripcion_diagnostico_movcargos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.descripcion_diagnostico_movcargos IS 'este campo almacena la descripcion del diagnostico';


--
-- Name: COLUMN osiris_erp_movcargos.lugar_de_ingreso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.lugar_de_ingreso IS 'Me indica el centro de costo el cual agrego este evento 1 es admision';


--
-- Name: COLUMN osiris_erp_movcargos.id_habitacion_cubiculo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_habitacion_cubiculo IS 'este campo se enlasa con tabla de habitacion o cubiculos his_habitacion_cubiculos';


--
-- Name: COLUMN osiris_erp_movcargos.vigente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.vigente IS 'nos indica si este movimiento esta vigente y cual es la habitacion actual de el paciente';


--
-- Name: COLUMN osiris_erp_movcargos.nombre_de_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.nombre_de_cirugia IS 'Almacena el nombre de la cirugia';


--
-- Name: COLUMN osiris_erp_movcargos.id_cie_10; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_cie_10 IS 'este campo se enlasa con la tabla de diagnosticos';


--
-- Name: COLUMN osiris_erp_movcargos.tipo_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.tipo_cirugia IS 'almacena el tipo de cirugia cuando se ingresa el paciente en admision';


--
-- Name: COLUMN osiris_erp_movcargos.diagnostico_primeravez; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.diagnostico_primeravez IS 'indica si el diagnostico dado es por primera vez';


--
-- Name: COLUMN osiris_erp_movcargos.id_anestesiologo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_anestesiologo IS 'se enlasa con la tabla de doctores';


--
-- Name: COLUMN osiris_erp_movcargos.id_tecnico_vision; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movcargos.id_tecnico_vision IS 'se enlasa con la tabla de empleados';


--
-- Name: osiris_erp_movcargos_id_movcargos_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_movcargos_id_movcargos_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_movcargos_id_movcargos_seq OWNER TO admin;

--
-- Name: osiris_erp_movcargos_id_movcargos_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_movcargos_id_movcargos_seq OWNED BY osiris_erp_movcargos.id_movcargos;


--
-- Name: osiris_erp_movimiento_documentos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_movimiento_documentos (
    pid_paciente integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    descripcion_documento character varying DEFAULT ''::bpchar,
    informacion_capturada character varying DEFAULT ''::bpchar,
    id_tipo_documento integer DEFAULT 1,
    id_secuencia integer NOT NULL
);


ALTER TABLE public.osiris_erp_movimiento_documentos OWNER TO admin;

--
-- Name: TABLE osiris_erp_movimiento_documentos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_movimiento_documentos IS 'Almacena los movimientos de los documentos capturados en Admision dependiendo el tipo de paciente';


--
-- Name: COLUMN osiris_erp_movimiento_documentos.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movimiento_documentos.pid_paciente IS 'este campo se enlasa con la tabla de pacientes';


--
-- Name: COLUMN osiris_erp_movimiento_documentos.id_tipo_documento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_movimiento_documentos.id_tipo_documento IS 'este campo se enlasa con la tabla tipos de documento';


--
-- Name: osiris_erp_movimiento_documentos_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_movimiento_documentos_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_movimiento_documentos_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_movimiento_documentos_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_movimiento_documentos_id_secuencia_seq OWNED BY osiris_erp_movimiento_documentos.id_secuencia;


--
-- Name: osiris_erp_notacredito_deta; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_notacredito_deta (
    numero_ntacred integer,
    descripcion_detalle character varying(100),
    total numeric(13,5),
    id_secuencia integer NOT NULL
);


ALTER TABLE public.osiris_erp_notacredito_deta OWNER TO admin;

--
-- Name: TABLE osiris_erp_notacredito_deta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_notacredito_deta IS 'Almacena el detalle de las notas de credito de las facturas';


--
-- Name: COLUMN osiris_erp_notacredito_deta.numero_ntacred; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_deta.numero_ntacred IS 'el numero de la nota de credito';


--
-- Name: COLUMN osiris_erp_notacredito_deta.descripcion_detalle; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_deta.descripcion_detalle IS 'indica descripcion del detalle';


--
-- Name: osiris_erp_notacredito_deta_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_notacredito_deta_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_notacredito_deta_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_notacredito_deta_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_notacredito_deta_id_secuencia_seq OWNED BY osiris_erp_notacredito_deta.id_secuencia;


--
-- Name: osiris_erp_notacredito_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_notacredito_enca (
    numero_factura integer,
    numero_ntacred integer,
    fecha_creacion_nota_credito date DEFAULT '2000-01-01'::date,
    sub_total_15 numeric(13,5) DEFAULT 0,
    sub_total_0 numeric(13,5) DEFAULT 0,
    iva_al_15 numeric(13,5) DEFAULT 0,
    cancelado boolean DEFAULT false,
    id_quien_creo character varying(15) DEFAULT ''::bpchar NOT NULL,
    total numeric(10,3) DEFAULT 0,
    pagada boolean DEFAULT false,
    fechahora_de_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_cancelo character varying(15),
    descripcion1 character varying DEFAULT ''::bpchar,
    descripcion2 character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_notacredito_enca OWNER TO admin;

--
-- Name: COLUMN osiris_erp_notacredito_enca.numero_factura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.numero_factura IS 'factura a la que se le aplica la nota de credito';


--
-- Name: COLUMN osiris_erp_notacredito_enca.numero_ntacred; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.numero_ntacred IS 'Es el numero de nota de credito';


--
-- Name: COLUMN osiris_erp_notacredito_enca.fecha_creacion_nota_credito; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.fecha_creacion_nota_credito IS 'Fecha en que se crea la nota';


--
-- Name: COLUMN osiris_erp_notacredito_enca.cancelado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.cancelado IS 'si fue cancelada la factura y/o nota';


--
-- Name: COLUMN osiris_erp_notacredito_enca.total; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.total IS 'total a credito ';


--
-- Name: COLUMN osiris_erp_notacredito_enca.fechahora_de_cancelacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.fechahora_de_cancelacion IS 'Almacena la hora y fecha en la que fue cancelada la nota';


--
-- Name: COLUMN osiris_erp_notacredito_enca.id_quien_cancelo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.id_quien_cancelo IS 'quien cancelo la factura';


--
-- Name: COLUMN osiris_erp_notacredito_enca.descripcion1; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.descripcion1 IS 'almacena alguna descripcion 1';


--
-- Name: COLUMN osiris_erp_notacredito_enca.descripcion2; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_notacredito_enca.descripcion2 IS 'almacena una segunda descripcion';


--
-- Name: osiris_erp_ordenes_compras_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_ordenes_compras_enca (
    id_orden_compra integer NOT NULL,
    id_proveedor integer,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fecha_solicitud date DEFAULT '2000-01-01'::date,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    numero_requisiciones character varying DEFAULT ''::bpchar,
    descripcion_proveedor character varying DEFAULT ''::bpchar,
    direccion_proveedor character varying DEFAULT ''::bpchar,
    telefonos_proveedor character varying DEFAULT ''::bpchar,
    contacto_proveedor character varying DEFAULT ''::bpchar,
    correo_electronico character varying DEFAULT ''::bpchar,
    representante_legal character varying DEFAULT ''::bpchar,
    rfc_proveedor character varying(20) DEFAULT ''::bpchar,
    faxnextel_proveedor character varying DEFAULT ''::bpchar,
    fecha_de_entrega date DEFAULT '2000-01-01'::date,
    lugar_de_entrega character varying DEFAULT ''::bpchar,
    condiciones_de_pago character varying DEFAULT ''::bpchar,
    lugar_de_recepcion character varying DEFAULT ''::bpchar,
    fechahora_recibida_almacen timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_recibio_almacen character varying(15) DEFAULT ''::bpchar,
    numero_orden_compra integer DEFAULT 0,
    embarque character varying DEFAULT ''::bpchar,
    factura_sin_ordencompra boolean DEFAULT false,
    subtotal_orden_compra numeric(13,5) DEFAULT 0,
    iva_orden_compra numeric(13,5) DEFAULT 0,
    total_orden_compra numeric(13,5) DEFAULT 0,
    dep_solicitante character varying DEFAULT ''::bpchar,
    tipo_orden_compra character varying DEFAULT ''::bpchar,
    id_emisor integer DEFAULT 1,
    cancelada boolean DEFAULT false,
    fechahora_cancelada timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_cancelo character varying DEFAULT ''::bpchar,
    observaciones character varying DEFAULT ''::bpchar,
    fecha_deorden_compra date DEFAULT '2000-01-01'::date
);


ALTER TABLE public.osiris_erp_ordenes_compras_enca OWNER TO admin;

--
-- Name: TABLE osiris_erp_ordenes_compras_enca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_ordenes_compras_enca IS 'Almacena las ordenes de compra realizadas';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.id_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.id_proveedor IS 'este campo se enlasa con la tabla de Proveedores';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.fechahora_creacion IS 'fecha y hora de creacion de orden de compra';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.fecha_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.fecha_solicitud IS 'almacena la fecha de cuando solicito la orden de compra';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.numero_requisiciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.numero_requisiciones IS 'almacena en forma de caracer las requisiciones que se solicitaron para la orden de compra';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.fechahora_recibida_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.fechahora_recibida_almacen IS 'almacena la fecha y la hora cuando se recibe la orden de compra con la factura en el lugar de origen';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.numero_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.numero_orden_compra IS 'almacena el numero de orden de compra creado';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.factura_sin_ordencompra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.factura_sin_ordencompra IS 'indica si la factura de proveedor ingreso al almacen sin una orden de compra';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.tipo_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.tipo_orden_compra IS 'almacena la el tipo de la orden de compra';


--
-- Name: COLUMN osiris_erp_ordenes_compras_enca.id_emisor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_ordenes_compras_enca.id_emisor IS 'este campo se enlasa con la tabla de emisores para CFD';


--
-- Name: osiris_erp_ordenes_compras_enca_dep_solicitante_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_ordenes_compras_enca_dep_solicitante_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_ordenes_compras_enca_dep_solicitante_seq OWNER TO admin;

--
-- Name: osiris_erp_ordenes_compras_enca_dep_solicitante_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_ordenes_compras_enca_dep_solicitante_seq OWNED BY osiris_erp_ordenes_compras_enca.dep_solicitante;


--
-- Name: osiris_erp_ordenes_compras_enca_id_orden_compra_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_ordenes_compras_enca_id_orden_compra_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_ordenes_compras_enca_id_orden_compra_seq OWNER TO admin;

--
-- Name: osiris_erp_ordenes_compras_enca_id_orden_compra_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_ordenes_compras_enca_id_orden_compra_seq OWNED BY osiris_erp_ordenes_compras_enca.id_orden_compra;


--
-- Name: osiris_erp_pases_qxurg; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_pases_qxurg (
    id_secuencia integer NOT NULL,
    pid_paciente integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_tipo_admisiones integer DEFAULT 0,
    id_quien_creo character varying DEFAULT ''::bpchar,
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying DEFAULT ''::bpchar,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    observaciones character varying DEFAULT ''::bpchar,
    id_tipo_paciente integer DEFAULT 0,
    id_empresa integer DEFAULT 1,
    id_aseguradora integer DEFAULT 1,
    motivo_eliminacion text DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_erp_pases_qxurg OWNER TO admin;

--
-- Name: TABLE osiris_erp_pases_qxurg; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_pases_qxurg IS 'almacena los pases de ingreso a departamentos medicos y de servicio cuando se requieran';


--
-- Name: COLUMN osiris_erp_pases_qxurg.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_pases_qxurg.id_tipo_admisiones IS 'este campo se enlasa con la tabla tipo_admsiones (departamentos)';


--
-- Name: COLUMN osiris_erp_pases_qxurg.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_pases_qxurg.eliminado IS 'indica si esta eliminado el pase';


--
-- Name: COLUMN osiris_erp_pases_qxurg.id_tipo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_pases_qxurg.id_tipo_paciente IS 'este campo se enlasa con la tabla tipo de pacientes';


--
-- Name: COLUMN osiris_erp_pases_qxurg.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_pases_qxurg.id_empresa IS 'este camnpo se enlasa con la tabla de empresas';


--
-- Name: COLUMN osiris_erp_pases_qxurg.id_aseguradora; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_pases_qxurg.id_aseguradora IS 'se enlasa con la tabla de aseguradoras';


--
-- Name: osiris_erp_pases_qxurg_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_pases_qxurg_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_pases_qxurg_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_pases_qxurg_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_pases_qxurg_id_secuencia_seq OWNED BY osiris_erp_pases_qxurg.id_secuencia;


--
-- Name: osiris_erp_proveedores; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_proveedores (
    descripcion_proveedor character varying(100) DEFAULT ''::bpchar,
    direccion_proveedor character varying(100) DEFAULT ''::bpchar,
    colonia_proveedor character varying(40) DEFAULT ''::bpchar,
    municipio_proveedor character varying(40) DEFAULT ''::bpchar,
    estado_proveedor character varying(40) DEFAULT ''::bpchar,
    rfc_proveedor character varying(20) DEFAULT ''::bpchar,
    curp_proveedor character varying(40) DEFAULT ''::bpchar,
    telefono1_proveedor character varying(20) DEFAULT ''::bpchar,
    telefono2_proveedor character varying(20) DEFAULT ''::bpchar,
    fax_proveedor character varying(20) DEFAULT ''::bpchar,
    mail_proveedor character varying(50) DEFAULT ''::bpchar,
    contacto1_proveedor character varying(50) DEFAULT ''::bpchar,
    contacto2_proveedor character varying(50) DEFAULT ''::bpchar,
    telefono_contacto1_proveedor character varying(20) DEFAULT ''::bpchar,
    telefono_contacto2_proveedor character varying(20) DEFAULT ''::bpchar,
    fechahora_creacion_proveedor timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    historial_cambios_proveedor text DEFAULT ''::bpchar,
    cp_proveedor character varying(10) DEFAULT ''::bpchar,
    fecha_baja_proveedor timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_baja character varying(15) DEFAULT ''::bpchar,
    proveedor_activo boolean DEFAULT true,
    id_forma_de_pago integer DEFAULT 1,
    celular_proveedor character varying(20) DEFAULT ''::bpchar,
    nextel_proveedor character varying(20) DEFAULT ''::bpchar,
    pagina_web_proveedor character varying(60) DEFAULT ''::bpchar,
    agrupacion4 character varying(3),
    id_proveedor integer NOT NULL,
    proveedor_lab boolean DEFAULT false,
    proveedor_img boolean DEFAULT false,
    localidad_proveedor character varying DEFAULT ''::bpchar,
    noexterior_proveedor character varying DEFAULT ''::bpchar,
    nointerior_proveedor character varying DEFAULT ''::bpchar,
    id_tipo_servicio integer DEFAULT 0
);


ALTER TABLE public.osiris_erp_proveedores OWNER TO admin;

--
-- Name: TABLE osiris_erp_proveedores; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_proveedores IS 'esta tabla almacena la informacion de todos los proveedores';


--
-- Name: COLUMN osiris_erp_proveedores.descripcion_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.descripcion_proveedor IS 'descripcion de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.direccion_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.direccion_proveedor IS 'direccion de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.colonia_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.colonia_proveedor IS 'colonia de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.municipio_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.municipio_proveedor IS 'municipio de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.estado_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.estado_proveedor IS 'estado de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.rfc_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.rfc_proveedor IS 'rfc de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.curp_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.curp_proveedor IS 'curp de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.telefono1_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.telefono1_proveedor IS 'telefono 1 de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.telefono2_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.telefono2_proveedor IS 'telefono 2 de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.fax_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.fax_proveedor IS 'fax de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.mail_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.mail_proveedor IS 'direccion de correo de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.contacto1_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.contacto1_proveedor IS 'contacto 1 de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.contacto2_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.contacto2_proveedor IS 'contacto 2 de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.telefono_contacto1_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.telefono_contacto1_proveedor IS 'telefono de el contacto 1 de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.telefono_contacto2_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.telefono_contacto2_proveedor IS 'telefono de el contacto 2 de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.fechahora_creacion_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.fechahora_creacion_proveedor IS 'fecha y hora de creacion de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.id_quien_creo IS 'identificador de quien creo al proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.historial_cambios_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.historial_cambios_proveedor IS 'historial de cambios de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.cp_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.cp_proveedor IS 'codigo postal de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.fecha_baja_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.fecha_baja_proveedor IS 'fecha de baja de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.id_quien_baja; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.id_quien_baja IS 'identificador de quien dio de baja al proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.proveedor_activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.proveedor_activo IS 'status de el proveedor (activo o inactivo)';


--
-- Name: COLUMN osiris_erp_proveedores.id_forma_de_pago; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.id_forma_de_pago IS 'este campo se enlaza con la tabla erp_forma_de_pago';


--
-- Name: COLUMN osiris_erp_proveedores.celular_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.celular_proveedor IS 'numero celular de el proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.nextel_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.nextel_proveedor IS 'numero de nextel del proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.pagina_web_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.pagina_web_proveedor IS 'direccion de la pagina web del proveedor';


--
-- Name: COLUMN osiris_erp_proveedores.proveedor_lab; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.proveedor_lab IS 'indica si el proveedor va ser de laboratorio esto es para realizar las solicitudes';


--
-- Name: COLUMN osiris_erp_proveedores.proveedor_img; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.proveedor_img IS 'indica que el proveedor sera de rayos x y de imagenologia';


--
-- Name: COLUMN osiris_erp_proveedores.id_tipo_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_proveedores.id_tipo_servicio IS 'se enlasa con la tabla tipo de servicio dado (servicios y consumo)';


--
-- Name: osiris_erp_proveedores_id_proveedor_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_proveedores_id_proveedor_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_proveedores_id_proveedor_seq OWNER TO admin;

--
-- Name: osiris_erp_proveedores_id_proveedor_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_proveedores_id_proveedor_seq OWNED BY osiris_erp_proveedores.id_proveedor;


--
-- Name: osiris_erp_puestos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_puestos (
    puesto character varying(50) DEFAULT ''::character varying,
    activo boolean DEFAULT false,
    id_puesto integer NOT NULL
);


ALTER TABLE public.osiris_erp_puestos OWNER TO admin;

--
-- Name: TABLE osiris_erp_puestos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_puestos IS 'Catalogo de Puestos';


--
-- Name: COLUMN osiris_erp_puestos.puesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_puestos.puesto IS 'Todos los puestos. Sin relacion al departamento';


--
-- Name: COLUMN osiris_erp_puestos.activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_puestos.activo IS 'True = si el puesto esta vigente';


--
-- Name: osiris_erp_puestos_id_puesto_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_puestos_id_puesto_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_puestos_id_puesto_seq OWNER TO admin;

--
-- Name: osiris_erp_puestos_id_puesto_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_puestos_id_puesto_seq OWNED BY osiris_erp_puestos.id_puesto;


--
-- Name: osiris_erp_referencias_pacientes; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_referencias_pacientes (
    id_ref integer,
    descripcion character varying,
    activo boolean,
    id_tipo_px integer
);


ALTER TABLE public.osiris_erp_referencias_pacientes OWNER TO admin;

--
-- Name: TABLE osiris_erp_referencias_pacientes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_referencias_pacientes IS 'Referencias de pacientes ';


--
-- Name: osiris_erp_remision_compra_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_remision_compra_enca (
    numero_orden_compra integer DEFAULT 0,
    fechahora_orden_compra timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    cancelado boolean DEFAULT false,
    fechahora_cancelado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_proveedor integer DEFAULT 1,
    remision_sin_orden_compra boolean DEFAULT false,
    numero_remision_proveedor character varying DEFAULT ''::character varying,
    id_quien_cancelo character varying(15) DEFAULT ''::bpchar,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fecha_remision date DEFAULT '2000-01-01'::date,
    numero_serie_cfd character varying DEFAULT ''::bpchar,
    ano_aprobacion_cfd character varying DEFAULT ''::bpchar,
    numero_aprobacion_cfd character varying DEFAULT ''::bpchar,
    numero_requisicion integer DEFAULT 0,
    subtotal_remision numeric(13,5) DEFAULT 0,
    iva_remision numeric(13,5) DEFAULT 0.00,
    total_remision numeric(13,5) DEFAULT 0,
    valida_conta boolean DEFAULT false,
    fechahora_valida_conta timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_validaconta character varying DEFAULT ''::bpchar,
    remisionada boolean DEFAULT false,
    fechahora_remision timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_remisiono character varying DEFAULT ''::bpchar,
    numero_remision integer DEFAULT 0,
    id_secuencia integer NOT NULL,
    fecha_entrada_almacen date DEFAULT '2000-01-01'::date,
    id_emisor integer DEFAULT 1
);


ALTER TABLE public.osiris_erp_remision_compra_enca OWNER TO admin;

--
-- Name: TABLE osiris_erp_remision_compra_enca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_remision_compra_enca IS 'Guarda informacion sobre las remisions de ordenes de compras';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.numero_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.numero_orden_compra IS 'este campo se enlasa con la tabla de ordenes de compra enca';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.remision_sin_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.remision_sin_orden_compra IS 'me indica si la remision ingresada es con o sin orden de compra';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.numero_serie_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.numero_serie_cfd IS 'almacena el numero de serie de la remision';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.ano_aprobacion_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.ano_aprobacion_cfd IS 'almacena el año de aprobacion de del cfd';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.numero_aprobacion_cfd; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.numero_aprobacion_cfd IS 'almacena el numero de aprobacion del cfd';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.valida_conta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.valida_conta IS 'bandera que indica si la remision fue validada por contabilidad';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.fechahora_valida_conta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.fechahora_valida_conta IS 'fecha y hora cuando valido contabilidad';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.remisionada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.remisionada IS 'bandera que indica s esta remisionada la remision del proveedor';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.fecha_entrada_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.fecha_entrada_almacen IS 'almacen la fecha cuando el proveer llegar a dejar la mercancia en almacen';


--
-- Name: COLUMN osiris_erp_remision_compra_enca.id_emisor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_remision_compra_enca.id_emisor IS 'este campo se enlasa con la tabla de emisores para CFD';


--
-- Name: osiris_erp_remision_compra_enca_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_remision_compra_enca_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_remision_compra_enca_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_remision_compra_enca_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_remision_compra_enca_id_secuencia_seq OWNED BY osiris_erp_remision_compra_enca.id_secuencia;


--
-- Name: osiris_erp_requisicion_deta; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_requisicion_deta (
    id_secuencia integer NOT NULL,
    id_requisicion integer DEFAULT 1,
    id_producto numeric(12,0),
    cantidad_solicitada numeric(13,5) DEFAULT 0.000,
    cantidad_comprada numeric(13,5) DEFAULT 0,
    fechahora_requisado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_requiso character varying(15) DEFAULT ''::bpchar,
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying(15) DEFAULT ''::bpchar,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_compro character varying(15) DEFAULT ''::bpchar,
    fechahora_compra timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    comprado boolean DEFAULT false,
    id_quien_recibio character varying(15) DEFAULT ''::character varying,
    fechahora_recibido timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    recibido boolean DEFAULT false,
    id_almacen integer DEFAULT 0,
    id_proveedor integer DEFAULT 1,
    numero_factura_proveedor character varying DEFAULT ''::character varying,
    costo_producto numeric(13,5) DEFAULT 0,
    costo_por_unidad numeric DEFAULT 0,
    precio_producto_publico numeric(13,5) DEFAULT 0,
    porcentage_ganancia numeric(7,3) DEFAULT 0,
    cantidad_de_embalaje numeric(6,2) DEFAULT 0,
    autorizada boolean DEFAULT false,
    id_quien_autorizo character varying(15) DEFAULT ''::bpchar,
    fechahora_autorizado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_tipo_admisiones integer,
    precio_proveedor_1 numeric(13,5) DEFAULT 0,
    precio_proveedor_2 numeric(13,5) DEFAULT 0,
    precio_proveedor_3 numeric(13,5) DEFAULT 0,
    id_proveedor1 integer DEFAULT 1,
    id_proveedor2 integer DEFAULT 1,
    id_proveedor3 integer DEFAULT 1,
    descripcion_proveedor2 character varying DEFAULT ''::bpchar,
    descripcion_proveedor3 character varying DEFAULT ''::bpchar,
    numero_orden_compra integer DEFAULT 0,
    precio_costo_prov_selec numeric(13,5) DEFAULT 0,
    precio_unitario_prov_selec numeric(13,5) DEFAULT 0,
    id_producto_proveedor character varying DEFAULT ''::character varying,
    factura_sin_orden_compra boolean DEFAULT false,
    cantidad_recibida numeric(13,5) DEFAULT 0,
    lote_producto character varying DEFAULT ''::character varying,
    caducidad_producto character varying DEFAULT ''::character varying,
    descripcion_producto_proveedor character varying DEFAULT ''::character varying,
    tipo_unidad_producto character varying DEFAULT ''::character varying,
    costo_producto_osiris numeric(13,5) DEFAULT 0,
    cantidad_de_embalaje_osiris numeric(6,2) DEFAULT 1,
    ingreso_por_requisicion boolean DEFAULT false,
    no_autorizada boolean DEFAULT false,
    fechahora_no_autorizada timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_no_autorizada character varying DEFAULT ''::bpchar,
    liberado_compra boolean DEFAULT true,
    id_emisor integer DEFAULT 1,
    cantidad_ingreso_stock numeric DEFAULT 0.00,
    porcentage_iva numeric(5,3) DEFAULT 0,
    fecha_deorden_compra date DEFAULT '2000-01-01'::date,
    cantidad_de_embalaje_compra numeric(6,2) DEFAULT 0,
    no_recibido_no_facturado boolean DEFAULT false,
    fechahora_no_rec_fact timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone
);


ALTER TABLE public.osiris_erp_requisicion_deta OWNER TO admin;

--
-- Name: TABLE osiris_erp_requisicion_deta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_requisicion_deta IS 'Esta tabla esta enlasada al requi enca y enca orden de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_requisicion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_requisicion IS 'este campo se enlasa con requisicion_enca';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_producto IS 'almacen el codigo del producto que se realiza la requisicion enlase con la tabla de productos';


--
-- Name: COLUMN osiris_erp_requisicion_deta.fechahora_requisado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.fechahora_requisado IS 'alamacen la fechay la hora de cuando realiza la requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_quien_requiso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_quien_requiso IS 'identificacion de quien solicita el pedido';


--
-- Name: COLUMN osiris_erp_requisicion_deta.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.eliminado IS 'bandera que indica si esta eliminado';


--
-- Name: COLUMN osiris_erp_requisicion_deta.fechahora_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.fechahora_compra IS 'fecha hora de cuando se realizo la orden de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.comprado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.comprado IS 'me indica si el producto fue comprado o se genero la orden de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.recibido; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.recibido IS 'estatus de si fue recibido en almacen';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_almacen IS 'almacen el almacen donde se recibio la mercancia';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_proveedor IS 'identificacion del proveedor enlase con la tabla de proveedores';


--
-- Name: COLUMN osiris_erp_requisicion_deta.costo_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.costo_producto IS 'almacena el costo general de un producto';


--
-- Name: COLUMN osiris_erp_requisicion_deta.costo_por_unidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.costo_por_unidad IS 'almacena el costo unitario del producto';


--
-- Name: COLUMN osiris_erp_requisicion_deta.precio_producto_publico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.precio_producto_publico IS 'alamacena el precio que se vende al publico';


--
-- Name: COLUMN osiris_erp_requisicion_deta.porcentage_ganancia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.porcentage_ganancia IS 'Almacena el porcentage de utilidad aplicado al producto';


--
-- Name: COLUMN osiris_erp_requisicion_deta.cantidad_de_embalaje; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.cantidad_de_embalaje IS 'almacen el embalaje que tiene el producto';


--
-- Name: COLUMN osiris_erp_requisicion_deta.autorizada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.autorizada IS 'bandera que indica si este producto esta autorizado para su compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_quien_autorizo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_quien_autorizo IS 'almacena el usuario que autorizo la compra de este material';


--
-- Name: COLUMN osiris_erp_requisicion_deta.fechahora_autorizado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.fechahora_autorizado IS 'almacena la hora y la fecha de cuando se autoriozo para su compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_tipo_admisiones IS 'almacena el centro de costo';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_proveedor1; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_proveedor1 IS 'almacen el id del proveedor de la requisicion se enlasa con la tabla de proveedores';


--
-- Name: COLUMN osiris_erp_requisicion_deta.numero_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.numero_orden_compra IS 'este campo se enlasa con la tabla de ordenes de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.precio_costo_prov_selec; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.precio_costo_prov_selec IS 'almacena el precio de costo del proveedor que se selecciono para hacer la orden de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.precio_unitario_prov_selec; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.precio_unitario_prov_selec IS 'precio unitario del proveedor que se selecciono para crear la orden de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_producto_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_producto_proveedor IS 'almacena el codigo del proveedor';


--
-- Name: COLUMN osiris_erp_requisicion_deta.factura_sin_orden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.factura_sin_orden_compra IS 'me indica si el concepto ingresado fue sin orden de compra y se enlasa a la tabla factura_enca_sin_orden_compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.cantidad_recibida; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.cantidad_recibida IS 'almacen la cantidad de productos recibidos en unidades segun la factura del proveedor';


--
-- Name: COLUMN osiris_erp_requisicion_deta.descripcion_producto_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.descripcion_producto_proveedor IS 'almacena el nombre del producto del proveedor';


--
-- Name: COLUMN osiris_erp_requisicion_deta.costo_producto_osiris; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.costo_producto_osiris IS 'almacena el precio de costo almacenado en la tabla de productos';


--
-- Name: COLUMN osiris_erp_requisicion_deta.cantidad_de_embalaje_osiris; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.cantidad_de_embalaje_osiris IS 'almacena el embalaje que esta almacenado en el catalogo de productos';


--
-- Name: COLUMN osiris_erp_requisicion_deta.ingreso_por_requisicion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.ingreso_por_requisicion IS 'bandera que indica que el ingreso de la factura se realizo mediante requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_deta.no_autorizada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.no_autorizada IS 'bandera que indica que el producto no se autorizo para su compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.fechahora_no_autorizada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.fechahora_no_autorizada IS 'fecha la cual no se autoriza';


--
-- Name: COLUMN osiris_erp_requisicion_deta.liberado_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.liberado_compra IS 'bandera que indica que el producto se libera para poder realizar un requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_deta.id_emisor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.id_emisor IS 'este campo se enlasa con la tabla de emisores para CFD';


--
-- Name: COLUMN osiris_erp_requisicion_deta.fecha_deorden_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.fecha_deorden_compra IS 'almacena la fecha de la orden de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.cantidad_de_embalaje_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.cantidad_de_embalaje_compra IS 'almacena el embalaje asigando cuando se realiza la orden de compra';


--
-- Name: COLUMN osiris_erp_requisicion_deta.no_recibido_no_facturado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_deta.no_recibido_no_facturado IS 'bandera que indica que este concepto no fue entregado por el proveedor ni tampoco lo facturo';


--
-- Name: osiris_erp_requisicion_deta_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_requisicion_deta_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_requisicion_deta_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_requisicion_deta_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_requisicion_deta_id_secuencia_seq OWNED BY osiris_erp_requisicion_deta.id_secuencia;


--
-- Name: osiris_erp_requisicion_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_requisicion_enca (
    id_requisicion integer NOT NULL,
    fechahora_creacion_requisicion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fecha_requerida date DEFAULT '2000-01-01'::date,
    id_tipo_admisiones integer,
    id_tipo_admisiones_cargada integer,
    autorizacion_para_comprar boolean DEFAULT false,
    fechahora_autorizacion_comprar timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_requiso character varying(15) DEFAULT ''::bpchar,
    observaciones character varying DEFAULT ''::bpchar,
    total_items_solicitados integer DEFAULT 0,
    total_items_comprados integer DEFAULT 0,
    descripcion_admisiones_cargada character varying DEFAULT ''::bpchar,
    id_quien_envio_compras character varying(15) DEFAULT ''::bpchar,
    requisicion_ordinaria boolean DEFAULT false,
    requisicion_urgente boolean DEFAULT false,
    descripcion_tipo_requisicion character varying DEFAULT ''::bpchar,
    enviada_a_compras boolean DEFAULT false,
    fechahora_envio_compras timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_envio_autorizacion_compra character varying(15),
    items_autorizados_paracomprar integer DEFAULT 0,
    id_proveedor integer DEFAULT 1,
    cancelado boolean DEFAULT false,
    id_quien_cancelo character varying(15) DEFAULT ''::bpchar,
    fechahora_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    motivo_requisicion character varying DEFAULT ''::bpchar,
    id_tipo_requisicion_compra integer DEFAULT 1,
    folio_de_servicio integer DEFAULT 0,
    pid_paciente integer DEFAULT 0
);


ALTER TABLE public.osiris_erp_requisicion_enca OWNER TO admin;

--
-- Name: TABLE osiris_erp_requisicion_enca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_requisicion_enca IS 'Esta tabla almana todas los encabezados de requisiones compras y almacen estatus actual';


--
-- Name: COLUMN osiris_erp_requisicion_enca.id_requisicion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.id_requisicion IS 'almacena el numero de requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_enca.fechahora_creacion_requisicion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.fechahora_creacion_requisicion IS 'almacena la fecha y la hora cuando se realizo la requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_enca.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.id_tipo_admisiones IS 'especifica el centro de costo donde se realizo la requisicion se enlasa con tabla his_tipo_admisiones';


--
-- Name: COLUMN osiris_erp_requisicion_enca.id_tipo_admisiones_cargada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.id_tipo_admisiones_cargada IS 'especifica el centro de costo el cual se va cargar la requisicion enlase con his_tipo_admisiones';


--
-- Name: COLUMN osiris_erp_requisicion_enca.autorizacion_para_comprar; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.autorizacion_para_comprar IS 'este campo idica si se envio para autorizacion la requisiscion';


--
-- Name: COLUMN osiris_erp_requisicion_enca.id_quien_requiso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.id_quien_requiso IS 'almacena el usuario quien requiso';


--
-- Name: COLUMN osiris_erp_requisicion_enca.observaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.observaciones IS 'almacen las observaciones que tiene la requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_enca.total_items_solicitados; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.total_items_solicitados IS 'almacena el total de los items que se solicitaron';


--
-- Name: COLUMN osiris_erp_requisicion_enca.total_items_comprados; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.total_items_comprados IS 'suma los items que se han realizado ordenes de compra';


--
-- Name: COLUMN osiris_erp_requisicion_enca.descripcion_admisiones_cargada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.descripcion_admisiones_cargada IS 'almacena la descripcion del centro de costo de donde se cargo la requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_enca.id_quien_envio_compras; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.id_quien_envio_compras IS 'almacen quien envio la requi para compras';


--
-- Name: COLUMN osiris_erp_requisicion_enca.requisicion_ordinaria; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.requisicion_ordinaria IS 'bandera que indica si la requisicion es ordinaria';


--
-- Name: COLUMN osiris_erp_requisicion_enca.requisicion_urgente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.requisicion_urgente IS 'bandera que indica si la requisicion es urgente';


--
-- Name: COLUMN osiris_erp_requisicion_enca.descripcion_tipo_requisicion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.descripcion_tipo_requisicion IS 'almacena la descripcion de tipo de requisicion';


--
-- Name: COLUMN osiris_erp_requisicion_enca.enviada_a_compras; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.enviada_a_compras IS 'este campo indica si la requisicion fue enviada a compras';


--
-- Name: COLUMN osiris_erp_requisicion_enca.items_autorizados_paracomprar; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.items_autorizados_paracomprar IS 'Contador de productos autorizados que se pueden comprar';


--
-- Name: COLUMN osiris_erp_requisicion_enca.id_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.id_proveedor IS 'este campo se enlasa con la tabla de proveedores';


--
-- Name: COLUMN osiris_erp_requisicion_enca.cancelado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.cancelado IS 'bandera que indica si la requisicion es cancelada';


--
-- Name: COLUMN osiris_erp_requisicion_enca.motivo_requisicion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.motivo_requisicion IS 'almacena el motivo por el cual se esta requisando';


--
-- Name: COLUMN osiris_erp_requisicion_enca.id_tipo_requisicion_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_requisicion_enca.id_tipo_requisicion_compra IS 'este campo se enlasa con la tabla de tipo de requisiciones de compra';


--
-- Name: osiris_erp_requisicion_enca_id_requisicion_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_requisicion_enca_id_requisicion_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_requisicion_enca_id_requisicion_seq OWNER TO admin;

--
-- Name: osiris_erp_requisicion_enca_id_requisicion_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_requisicion_enca_id_requisicion_seq OWNED BY osiris_erp_requisicion_enca.id_requisicion;


--
-- Name: osiris_erp_reservaciones; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_reservaciones (
    pid_paciente integer DEFAULT 0,
    id_quien_reservo character varying(15) DEFAULT ''::bpchar,
    fechahora_reservacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    cancelado boolean DEFAULT false,
    id_quien_cancelo character varying(15) DEFAULT ''::bpchar,
    fechahora_cancelado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    valor_paquete numeric DEFAULT 0,
    id_presupuesto integer DEFAULT 1 NOT NULL,
    id_tipo_cirugia integer DEFAULT 1 NOT NULL,
    fecha_hora_liberacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    motivo_cancelacion character varying DEFAULT ''::bpchar,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_libero character varying(15) DEFAULT ''::bpchar,
    folio_de_servicio integer DEFAULT 0,
    id_secuencia integer NOT NULL,
    dias_internamiento integer DEFAULT 0
);


ALTER TABLE public.osiris_erp_reservaciones OWNER TO admin;

--
-- Name: TABLE osiris_erp_reservaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_reservaciones IS 'guarda informacion sobre la reservacion de paquetes';


--
-- Name: COLUMN osiris_erp_reservaciones.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_reservaciones.pid_paciente IS 'se enlasa con la tabla de pacientes';


--
-- Name: COLUMN osiris_erp_reservaciones.id_presupuesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_reservaciones.id_presupuesto IS 'id del presupuesto se enlasa con la tabla presupuesto_enca';


--
-- Name: COLUMN osiris_erp_reservaciones.id_tipo_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_reservaciones.id_tipo_cirugia IS 'se enlasa con la tabla his_tipo_cirugias';


--
-- Name: COLUMN osiris_erp_reservaciones.fecha_hora_liberacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_reservaciones.fecha_hora_liberacion IS 'muestra la fecha, hora de liberacion';


--
-- Name: COLUMN osiris_erp_reservaciones.motivo_cancelacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_reservaciones.motivo_cancelacion IS 'motivo por cual se cancelo';


--
-- Name: COLUMN osiris_erp_reservaciones.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_reservaciones.fechahora_creacion IS 'muestra la hora y fecha de creacion(proseso)';


--
-- Name: COLUMN osiris_erp_reservaciones.id_quien_libero; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_reservaciones.id_quien_libero IS 'almacen al usuario quien libero la reservacion';


--
-- Name: osiris_erp_reservaciones_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_reservaciones_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_reservaciones_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_erp_reservaciones_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_reservaciones_id_secuencia_seq OWNED BY osiris_erp_reservaciones.id_secuencia;


--
-- Name: osiris_erp_sucursales; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_sucursales (
    id_sucursal integer NOT NULL,
    descripcion_sucursal character varying DEFAULT ''::bpchar,
    siglas_sucursal character varying DEFAULT ''::bpchar,
    id_municipio integer DEFAULT 1,
    activo boolean DEFAULT true
);


ALTER TABLE public.osiris_erp_sucursales OWNER TO admin;

--
-- Name: TABLE osiris_erp_sucursales; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_sucursales IS 'tabla de sucursales hospitalarias';


--
-- Name: COLUMN osiris_erp_sucursales.id_municipio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_sucursales.id_municipio IS 'este campo se enlasa con la tabla de municipios y de estados';


--
-- Name: COLUMN osiris_erp_sucursales.activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_sucursales.activo IS 'indica si esta activa la sucursal';


--
-- Name: osiris_erp_sucursales_id_sucursal_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_sucursales_id_sucursal_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_sucursales_id_sucursal_seq OWNER TO admin;

--
-- Name: osiris_erp_sucursales_id_sucursal_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_sucursales_id_sucursal_seq OWNED BY osiris_erp_sucursales.id_sucursal;


--
-- Name: osiris_erp_tabla_ganancia; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_tabla_ganancia (
    id_secuancia integer NOT NULL,
    precio_costo_inicial numeric(10,3) DEFAULT 0,
    precio_costo_final numeric(10,3) DEFAULT 0,
    porcentage_de_ganancia numeric(13,3) DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15),
    historial_de_movimiento text DEFAULT ''::bpchar,
    activo boolean DEFAULT true
);


ALTER TABLE public.osiris_erp_tabla_ganancia OWNER TO admin;

--
-- Name: TABLE osiris_erp_tabla_ganancia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_tabla_ganancia IS 'Esta tabla almacena el porcentage de ganancia aplicado para cada producto';


--
-- Name: COLUMN osiris_erp_tabla_ganancia.id_secuancia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tabla_ganancia.id_secuancia IS 'seciencia de la tabla';


--
-- Name: COLUMN osiris_erp_tabla_ganancia.precio_costo_inicial; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tabla_ganancia.precio_costo_inicial IS 'rango incial para la comparacion de costos';


--
-- Name: COLUMN osiris_erp_tabla_ganancia.precio_costo_final; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tabla_ganancia.precio_costo_final IS 'rango final para la compraracion de costos';


--
-- Name: COLUMN osiris_erp_tabla_ganancia.porcentage_de_ganancia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tabla_ganancia.porcentage_de_ganancia IS 'Almacena el porcentage de ganancia que aplicara a los productos';


--
-- Name: COLUMN osiris_erp_tabla_ganancia.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tabla_ganancia.id_quien_creo IS 'almacena quien creo';


--
-- Name: COLUMN osiris_erp_tabla_ganancia.historial_de_movimiento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tabla_ganancia.historial_de_movimiento IS 'almacena el historial de movimientos realizados';


--
-- Name: osiris_erp_tabla_ganancia_id_secuancia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_tabla_ganancia_id_secuancia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_tabla_ganancia_id_secuancia_seq OWNER TO admin;

--
-- Name: osiris_erp_tabla_ganancia_id_secuancia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_tabla_ganancia_id_secuancia_seq OWNED BY osiris_erp_tabla_ganancia.id_secuancia;


--
-- Name: osiris_erp_tipo_requisiciones_compra; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_tipo_requisiciones_compra (
    id_tipo_requisicion_compra integer NOT NULL,
    descripcion_tipo_requisicion character varying DEFAULT ''::bpchar,
    activo boolean DEFAULT true,
    selecciona_paciente boolean DEFAULT false,
    caja_chica boolean DEFAULT false
);


ALTER TABLE public.osiris_erp_tipo_requisiciones_compra OWNER TO admin;

--
-- Name: TABLE osiris_erp_tipo_requisiciones_compra; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_tipo_requisiciones_compra IS 'Esta tabla almcena los tipo de requisiciones para comprar';


--
-- Name: COLUMN osiris_erp_tipo_requisiciones_compra.selecciona_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tipo_requisiciones_compra.selecciona_paciente IS ' 	bandera que indica que debe seleccionar un numero de atencion';


--
-- Name: COLUMN osiris_erp_tipo_requisiciones_compra.caja_chica; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_erp_tipo_requisiciones_compra.caja_chica IS 'indica que el tipo de requi es de caja chica';


--
-- Name: osiris_erp_tipo_requisiciones_co_id_tipo_requisicion_compra_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_tipo_requisiciones_co_id_tipo_requisicion_compra_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_tipo_requisiciones_co_id_tipo_requisicion_compra_seq OWNER TO admin;

--
-- Name: osiris_erp_tipo_requisiciones_co_id_tipo_requisicion_compra_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_tipo_requisiciones_co_id_tipo_requisicion_compra_seq OWNED BY osiris_erp_tipo_requisiciones_compra.id_tipo_requisicion_compra;


--
-- Name: osiris_erp_tipo_servicios; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_erp_tipo_servicios (
    id_tipo_servicio integer NOT NULL,
    descripcion_tipo_servcio character varying DEFAULT ''::bpchar,
    activo boolean DEFAULT true
);


ALTER TABLE public.osiris_erp_tipo_servicios OWNER TO admin;

--
-- Name: TABLE osiris_erp_tipo_servicios; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_erp_tipo_servicios IS 'almacena lo tipo de servicios que se otorgan servicios consumo etc.';


--
-- Name: osiris_erp_tipo_servicios_id_tipo_servicio_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_erp_tipo_servicios_id_tipo_servicio_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_erp_tipo_servicios_id_tipo_servicio_seq OWNER TO admin;

--
-- Name: osiris_erp_tipo_servicios_id_tipo_servicio_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_erp_tipo_servicios_id_tipo_servicio_seq OWNED BY osiris_erp_tipo_servicios.id_tipo_servicio;


--
-- Name: osiris_estados; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_estados (
    id_estado integer DEFAULT nextval(('public.osiris_estados_id_secuencia_seq'::text)::regclass) NOT NULL,
    descripcion_estado character varying
);


ALTER TABLE public.osiris_estados OWNER TO admin;

--
-- Name: TABLE osiris_estados; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_estados IS 'tabla de estados del pais';


--
-- Name: COLUMN osiris_estados.id_estado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_estados.id_estado IS 'id del estado';


--
-- Name: COLUMN osiris_estados.descripcion_estado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_estados.descripcion_estado IS 'noombre del estado';


--
-- Name: osiris_estados_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_estados_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_estados_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_grupo1_producto; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_grupo1_producto (
    id_grupo1_producto integer NOT NULL,
    descripcion_grupo1_producto character varying(45),
    activo boolean DEFAULT false
);


ALTER TABLE public.osiris_grupo1_producto OWNER TO admin;

--
-- Name: TABLE osiris_grupo1_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_grupo1_producto IS 'Sub-grupo1 para productos';


--
-- Name: osiris_grupo1_producto_id_grupo1_producto_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_grupo1_producto_id_grupo1_producto_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_grupo1_producto_id_grupo1_producto_seq OWNER TO admin;

--
-- Name: osiris_grupo1_producto_id_grupo1_producto_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_grupo1_producto_id_grupo1_producto_seq OWNED BY osiris_grupo1_producto.id_grupo1_producto;


--
-- Name: osiris_grupo2_producto; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_grupo2_producto (
    id_grupo2_producto integer NOT NULL,
    descripcion_grupo2_producto character varying,
    activo boolean DEFAULT true
);


ALTER TABLE public.osiris_grupo2_producto OWNER TO admin;

--
-- Name: TABLE osiris_grupo2_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_grupo2_producto IS 'sub-grupo 2 para la division de productos';


--
-- Name: osiris_grupo2_producto_id_grupo2_producto_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_grupo2_producto_id_grupo2_producto_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_grupo2_producto_id_grupo2_producto_seq OWNER TO admin;

--
-- Name: osiris_grupo2_producto_id_grupo2_producto_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_grupo2_producto_id_grupo2_producto_seq OWNED BY osiris_grupo2_producto.id_grupo2_producto;


--
-- Name: osiris_grupo_producto; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_grupo_producto (
    id_grupo_producto integer,
    descripcion_grupo_producto character varying(45) DEFAULT ''::bpchar,
    agrupacion character varying(3) DEFAULT ''::bpchar,
    agrupacion2 character varying(3) DEFAULT ''::bpchar,
    agrupacion3 character varying(3) DEFAULT ''::bpchar,
    agrupacion4 character varying(3) DEFAULT ''::bpchar,
    porcentage_utilidad_grupo numeric(8,3) DEFAULT 0,
    cuenta_mayor_ingreso integer,
    cuenta_mayor_egreso integer,
    id_secuencia1 integer NOT NULL,
    agrupacion_4 boolean DEFAULT false,
    activo boolean DEFAULT true,
    agrupacion5 character varying(3) DEFAULT ''::bpchar,
    agrupacion6 character varying(3) DEFAULT ''::bpchar,
    agrupacion7 character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_grupo_producto OWNER TO admin;

--
-- Name: TABLE osiris_grupo_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_grupo_producto IS 'es el grupo de los productos';


--
-- Name: COLUMN osiris_grupo_producto.agrupacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_grupo_producto.agrupacion IS 'este campo me sirve para saber que agrupacion tiene, md1 puede rebajar en sub-almacen y poder determinar las busquedas de productos';


--
-- Name: COLUMN osiris_grupo_producto.agrupacion4; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_grupo_producto.agrupacion4 IS 'me indican que estos productos se puedan buscar en almacen';


--
-- Name: COLUMN osiris_grupo_producto.porcentage_utilidad_grupo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_grupo_producto.porcentage_utilidad_grupo IS 'especifica el porcentage de ganancia que se va tener por cada grupo';


--
-- Name: COLUMN osiris_grupo_producto.cuenta_mayor_ingreso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_grupo_producto.cuenta_mayor_ingreso IS 'almacena el numero de cuenta mayor contable de los ingresos';


--
-- Name: COLUMN osiris_grupo_producto.cuenta_mayor_egreso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_grupo_producto.cuenta_mayor_egreso IS 'almacena el numero de cuenta mayor contable de los egresos';


--
-- Name: COLUMN osiris_grupo_producto.agrupacion7; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_grupo_producto.agrupacion7 IS 'grupo de nuticion para poder solicitar en modulos medicos las dietas a los pacientes';


--
-- Name: osiris_grupo_producto_id_secuencia1_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_grupo_producto_id_secuencia1_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_grupo_producto_id_secuencia1_seq OWNER TO admin;

--
-- Name: osiris_grupo_producto_id_secuencia1_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_grupo_producto_id_secuencia1_seq OWNED BY osiris_grupo_producto.id_secuencia1;


--
-- Name: osiris_his_calendario_citaqx; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_calendario_citaqx (
    pid_paciente integer DEFAULT 1,
    folio_de_servicio integer DEFAULT 1,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_cirujano integer DEFAULT 0,
    id_neonatologo integer DEFAULT 0,
    id_ayudante integer DEFAULT 0,
    id_anestesiologo integer DEFAULT 0,
    id_circulante1 integer DEFAULT 0,
    id_circulante2 integer DEFAULT 0,
    id_internista integer DEFAULT 0,
    id_tipo_cirugia integer DEFAULT 1,
    id_diagnostico integer DEFAULT 1,
    observaciones character varying(200) DEFAULT ''::bpchar,
    instrumentacion_especial character varying(200) DEFAULT ''::bpchar,
    inicio_cirugia character varying(5) DEFAULT '00:00'::character varying,
    termino_cirugia character varying(5) DEFAULT '00:00'::character varying,
    entrada_sala character varying(5) DEFAULT '00:00'::character varying,
    salida_sala character varying(5) DEFAULT '00:00'::character varying,
    cirujano character varying(60) DEFAULT ''::bpchar,
    neonatologo character varying(60) DEFAULT ''::bpchar,
    ayudante character varying(60) DEFAULT ''::bpchar,
    anestesiologo character varying(60) DEFAULT ''::bpchar,
    circulante1 character varying(60) DEFAULT ''::bpchar,
    circulante2 character varying(60) DEFAULT ''::bpchar,
    internista character varying(60) DEFAULT ''::bpchar,
    id_presupuesto integer DEFAULT 1,
    id_medico integer DEFAULT 0,
    fecha_programacion date DEFAULT '2000-01-01'::date,
    hora_programacion character varying(5) DEFAULT '00:00'::character varying,
    aseguradora character varying(60) DEFAULT ''::bpchar,
    diagnostico character varying(100) DEFAULT ''::bpchar,
    cirugia character varying(100) DEFAULT ''::bpchar,
    nombre_medico character varying(90) DEFAULT ''::bpchar,
    descripcion_especialidad character varying(60) DEFAULT ''::bpchar,
    especialidad_cirugia character varying(60) DEFAULT ''::bpchar,
    tipo_anestecia character varying(60) DEFAULT ''::bpchar,
    cancelado boolean DEFAULT false,
    referido_por character varying DEFAULT ''::bpchar,
    motivo_consulta character varying DEFAULT ''::bpchar,
    id_tipocita integer DEFAULT 1,
    nombre_paciente character varying(110) DEFAULT ''::bpchar,
    sexo_paciente character varying(1) DEFAULT ''::bpchar,
    estado_civil_paciente character varying(25) DEFAULT ''::bpchar,
    email_paciente character varying(60) DEFAULT ''::bpchar,
    celular1_paciente character varying(30) DEFAULT ''::bpchar,
    id_habitacion integer DEFAULT 1,
    id_habitacion1 integer DEFAULT 1,
    descripcion_qx_utilizado character varying(60) DEFAULT ''::bpchar,
    id_numero_citaqx integer DEFAULT 0,
    id_secuencia integer NOT NULL,
    fecha_nacimiento_paciente date DEFAULT '2000-01-01'::date,
    id_tipo_paciente integer DEFAULT 0,
    id_tipo_admisiones integer DEFAULT 0,
    id_aseguradora integer DEFAULT 1,
    id_empresa integer DEFAULT 1,
    id_especialidad integer DEFAULT 1,
    telefono_paciente character varying DEFAULT ''::bpchar,
    id_quiencreo_cita character varying DEFAULT ''::bpchar,
    motivo_cancelacion_citaqx character varying DEFAULT ''::bpchar,
    fechahora_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_cancelo character varying DEFAULT ''::bpchar,
    reagendado boolean DEFAULT false,
    fechahora_reagendado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_reagendado character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_calendario_citaqx OWNER TO admin;

--
-- Name: TABLE osiris_his_calendario_citaqx; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_calendario_citaqx IS 'esta tabla almacenara calendario asignado desde el modulo de citas y quirofano';


--
-- Name: COLUMN osiris_his_calendario_citaqx.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.pid_paciente IS 'pid asignado en admision';


--
-- Name: COLUMN osiris_his_calendario_citaqx.folio_de_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.folio_de_servicio IS 'folio asignado en admision';


--
-- Name: COLUMN osiris_his_calendario_citaqx.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.fechahora_creacion IS 'almacena fecha y hora en la que se separo el quirofano';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_cirujano; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_cirujano IS 'almacena el id del cirujano';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_ayudante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_ayudante IS 'almacena id ayudante';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_anestesiologo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_anestesiologo IS 'almacena id anesteciologo';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_circulante1; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_circulante1 IS 'alamacena id circulante 1';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_circulante2; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_circulante2 IS 'almacena id circulante 2';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_internista; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_internista IS 'almacena id internista';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_tipo_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_tipo_cirugia IS 'almacena id cirugia se enlaza con la tabla his_tipo_cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_diagnostico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_diagnostico IS 'almacena id diagnostico se enlaza con la tabla his_tipo_diagnosticos';


--
-- Name: COLUMN osiris_his_calendario_citaqx.observaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.observaciones IS 'notas de la cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.instrumentacion_especial; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.instrumentacion_especial IS 'notifica que tipo de instrumentos utilizaron en la cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.inicio_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.inicio_cirugia IS 'almacena la hora de inicio de la cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.termino_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.termino_cirugia IS 'almacena la hora de termino de la cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.entrada_sala; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.entrada_sala IS 'almacena la hora de entrada a la sala';


--
-- Name: COLUMN osiris_his_calendario_citaqx.salida_sala; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.salida_sala IS 'almacena la hora de salida de la sala';


--
-- Name: COLUMN osiris_his_calendario_citaqx.cirujano; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.cirujano IS 'almacena el nombre del cirujano';


--
-- Name: COLUMN osiris_his_calendario_citaqx.neonatologo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.neonatologo IS 'almacena el nombre del neonatologo';


--
-- Name: COLUMN osiris_his_calendario_citaqx.ayudante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.ayudante IS 'almacena el nombre del ayudante';


--
-- Name: COLUMN osiris_his_calendario_citaqx.anestesiologo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.anestesiologo IS 'almacena el nombre del anestesiologo';


--
-- Name: COLUMN osiris_his_calendario_citaqx.circulante1; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.circulante1 IS 'almacena el nombre del primer circulante';


--
-- Name: COLUMN osiris_his_calendario_citaqx.circulante2; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.circulante2 IS 'almacena el nombre del segundo circulante';


--
-- Name: COLUMN osiris_his_calendario_citaqx.internista; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.internista IS 'almacena el nombre del internista';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_presupuesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_presupuesto IS 'almacena el numero de reservacion cuando es paquete';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_medico IS 'almacena id medico';


--
-- Name: COLUMN osiris_his_calendario_citaqx.aseguradora; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.aseguradora IS 'almacena el nombre de la aseguradora del paciente';


--
-- Name: COLUMN osiris_his_calendario_citaqx.diagnostico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.diagnostico IS 'almacena el diagnostico para la cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.cirugia IS 'almacena el tipo de cirugia a realizar';


--
-- Name: COLUMN osiris_his_calendario_citaqx.nombre_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.nombre_medico IS 'almacena el nombre del medico';


--
-- Name: COLUMN osiris_his_calendario_citaqx.descripcion_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.descripcion_especialidad IS 'almacen la especialidad del medico';


--
-- Name: COLUMN osiris_his_calendario_citaqx.especialidad_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.especialidad_cirugia IS 'almacena la especialidad de la cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.cancelado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.cancelado IS 'se utiliza para saber si se a cacelado una cita o cirugia';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_tipocita; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_tipocita IS '1 Cita a paciente 2 cita a quirofano QX';


--
-- Name: COLUMN osiris_his_calendario_citaqx.nombre_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.nombre_paciente IS 'nombre del paciente cuando no tiene PID';


--
-- Name: COLUMN osiris_his_calendario_citaqx.sexo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.sexo_paciente IS 'H Hombre M Mujer';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_habitacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_habitacion IS 'se enlasa con la tabla de habitaciones (consultorio o quirofano)';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_habitacion1; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_habitacion1 IS 'quirofafon que se utilizo se enlaza con la tabla habitaciones';


--
-- Name: COLUMN osiris_his_calendario_citaqx.descripcion_qx_utilizado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.descripcion_qx_utilizado IS 'almacena la descripcion del quirofano que se utilizo';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_numero_citaqx; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_numero_citaqx IS 'alamcena los numero de consulta de citas y de quirofano';


--
-- Name: COLUMN osiris_his_calendario_citaqx.fecha_nacimiento_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.fecha_nacimiento_paciente IS 'alamcena la fecha de nacimiento del paciente';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_tipo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_tipo_paciente IS 'este campo se enlasa con la tabla de tipo_pacientes';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_tipo_admisiones IS 'este campo se enlasa con la tabla tipo_admisiones QX ENF LAB';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_aseguradora; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_aseguradora IS 'este campo se enlasa con tabla de aseguradoras';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_empresa IS 'este campo se enlasa con la tabla de empresas';


--
-- Name: COLUMN osiris_his_calendario_citaqx.id_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_calendario_citaqx.id_especialidad IS 'este campo se enlasa a la tabla de tipo_especialidad';


--
-- Name: osiris_his_calendario_citaqx_backup; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_calendario_citaqx_backup (
    pid_paciente integer DEFAULT 1,
    folio_de_servicio integer DEFAULT 1,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_cirujano integer DEFAULT 0,
    id_neonatologo integer DEFAULT 0,
    id_ayudante integer DEFAULT 0,
    id_anestesiologo integer DEFAULT 0,
    id_circulante1 integer DEFAULT 0,
    id_circulante2 integer DEFAULT 0,
    id_internista integer DEFAULT 0,
    id_tipo_cirugia integer DEFAULT 1,
    id_diagnostico integer DEFAULT 1,
    observaciones character varying(200) DEFAULT ''::bpchar,
    instrumentacion_especial character varying(200) DEFAULT ''::bpchar,
    inicio_cirugia character varying(5) DEFAULT '00:00'::character varying,
    termino_cirugia character varying(5) DEFAULT '00:00'::character varying,
    entrada_sala character varying(5) DEFAULT '00:00'::character varying,
    salida_sala character varying(5) DEFAULT '00:00'::character varying,
    cirujano character varying(60) DEFAULT ''::bpchar,
    neonatologo character varying(60) DEFAULT ''::bpchar,
    ayudante character varying(60) DEFAULT ''::bpchar,
    anestesiologo character varying(60) DEFAULT ''::bpchar,
    circulante1 character varying(60) DEFAULT ''::bpchar,
    circulante2 character varying(60) DEFAULT ''::bpchar,
    internista character varying(60) DEFAULT ''::bpchar,
    id_presupuesto integer DEFAULT 1,
    id_medico integer DEFAULT 0,
    fecha_programacion date DEFAULT '2000-01-01'::date,
    hora_programacion character varying(5) DEFAULT '00:00'::character varying,
    aseguradora character varying(60) DEFAULT ''::bpchar,
    diagnostico character varying(100) DEFAULT ''::bpchar,
    cirugia character varying(100) DEFAULT ''::bpchar,
    nombre_medico character varying(90) DEFAULT ''::bpchar,
    descripcion_especialidad character varying(60) DEFAULT ''::bpchar,
    especialidad_cirugia character varying(60) DEFAULT ''::bpchar,
    tipo_anestecia character varying(60) DEFAULT ''::bpchar,
    cancelado boolean DEFAULT false,
    referido_por character varying DEFAULT ''::bpchar,
    motivo_consulta character varying DEFAULT ''::bpchar,
    id_tipocita integer DEFAULT 1,
    nombre_paciente character varying(110) DEFAULT ''::bpchar,
    sexo_paciente character varying(1) DEFAULT ''::bpchar,
    estado_civil_paciente character varying(25) DEFAULT ''::bpchar,
    email_paciente character varying(60) DEFAULT ''::bpchar,
    celular1_paciente character varying(30) DEFAULT ''::bpchar,
    id_habitacion integer DEFAULT 1,
    id_habitacion1 integer DEFAULT 1,
    descripcion_qx_utilizado character varying(60) DEFAULT ''::bpchar,
    id_numero_citaqx integer DEFAULT 0,
    id_secuencia integer NOT NULL,
    fecha_nacimiento_paciente date DEFAULT '2000-01-01'::date,
    id_tipo_paciente integer DEFAULT 0,
    id_tipo_admisiones integer DEFAULT 0,
    id_aseguradora integer DEFAULT 1,
    id_empresa integer DEFAULT 1,
    id_especialidad integer DEFAULT 1,
    telefono_paciente character varying DEFAULT ''::bpchar,
    id_quiencreo_cita character varying DEFAULT ''::bpchar,
    motivo_cancelacion_citaqx character varying DEFAULT ''::bpchar,
    fechahora_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_cancelo character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_calendario_citaqx_backup OWNER TO admin;

--
-- Name: osiris_his_calendario_citaqx_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_calendario_citaqx_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_calendario_citaqx_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_calendario_citaqx_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_calendario_citaqx_id_secuencia_seq OWNED BY osiris_his_calendario_citaqx.id_secuencia;


--
-- Name: osiris_his_cirugias_deta; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_cirugias_deta (
    id_producto numeric(12,0),
    id_tipo_cirugia integer,
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15),
    id_secuencia integer NOT NULL,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_empleado character varying(15),
    cantidad_aplicada numeric(13,6) DEFAULT 0,
    id_tipo_admisiones integer
);


ALTER TABLE public.osiris_his_cirugias_deta OWNER TO admin;

--
-- Name: TABLE osiris_his_cirugias_deta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_cirugias_deta IS 'Almacena el detalle de materiales que se usan para realizar una cirugia';


--
-- Name: COLUMN osiris_his_cirugias_deta.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_cirugias_deta.id_producto IS 'este campo esta enlasado con la tabla de productos';


--
-- Name: COLUMN osiris_his_cirugias_deta.id_tipo_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_cirugias_deta.id_tipo_cirugia IS 'se enlasa con la tabla tipo_cirugia';


--
-- Name: COLUMN osiris_his_cirugias_deta.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_cirugias_deta.eliminado IS 'bandera si el producto esta devuelto para que no se cobre al paciente (devolucion)';


--
-- Name: COLUMN osiris_his_cirugias_deta.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_cirugias_deta.fechahora_creacion IS 'se almacena la fecha y la hora cuando se realizo este cargo al paciente';


--
-- Name: COLUMN osiris_his_cirugias_deta.id_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_cirugias_deta.id_empleado IS 'quien realizo cargo';


--
-- Name: COLUMN osiris_his_cirugias_deta.cantidad_aplicada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_cirugias_deta.cantidad_aplicada IS 'Almacena la cantidad aplicada ';


--
-- Name: COLUMN osiris_his_cirugias_deta.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_cirugias_deta.id_tipo_admisiones IS 'se enlaza a la tabla his_tipo_admisiones para ver el lugar donde se aplicara el producto';


--
-- Name: osiris_his_cirugias_deta_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_cirugias_deta_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_cirugias_deta_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_cirugias_deta_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_cirugias_deta_id_secuencia_seq OWNED BY osiris_his_cirugias_deta.id_secuencia;


--
-- Name: osiris_his_examenes_laboratorio; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_examenes_laboratorio (
    id_producto numeric(12,0),
    parametro character varying(150) DEFAULT ''::bpchar,
    valor_referencia character varying(150) DEFAULT ''::bpchar,
    aplica_valor_referencia boolean,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    id_secuencia integer NOT NULL,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-01 00:00:00'::timestamp without time zone,
    id_secuencia_estudio integer DEFAULT 0,
    id_secuencia_parametros integer DEFAULT 0,
    tipo_valor_referencia character varying(1) DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_examenes_laboratorio OWNER TO admin;

--
-- Name: TABLE osiris_his_examenes_laboratorio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_examenes_laboratorio IS 'Tabla que guarda la informacion de los tipos de examenes que se realizan en el laboratorio ';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.id_producto IS 'codigo de el producto de el laboratorio';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.parametro; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.parametro IS 'parametro fijo de el resultado del examen';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.valor_referencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.valor_referencia IS 'valor de referencia de el examen del laboratorio';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.aplica_valor_referencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.aplica_valor_referencia IS 'campo para aplicar el valor de referencia';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.id_quien_creo IS 'id de quien creo el examen';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.fechahora_creacion IS 'almacena la fecha de creacion';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.id_secuencia_estudio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.id_secuencia_estudio IS 'este campo sirve para orden cada estudio de laboratorio';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.id_secuencia_parametros; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.id_secuencia_parametros IS 'secuencia para mas parametros del sexo y de edad del paciente';


--
-- Name: COLUMN osiris_his_examenes_laboratorio.tipo_valor_referencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_examenes_laboratorio.tipo_valor_referencia IS 'M = masculino F= femenino N=niño';


--
-- Name: osiris_his_examenes_laboratorio_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_examenes_laboratorio_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_examenes_laboratorio_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_examenes_laboratorio_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_examenes_laboratorio_id_secuencia_seq OWNED BY osiris_his_examenes_laboratorio.id_secuencia;


--
-- Name: osiris_his_explfis_parametros; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_explfis_parametros (
    id_tipo_admision integer DEFAULT 0,
    descripcion_parametro character varying DEFAULT ''::bpchar,
    valor_default character varying DEFAULT ''::bpchar,
    activo boolean DEFAULT true,
    id_parametro numeric DEFAULT 0,
    id_secuencia_parametro integer DEFAULT 1,
    id_especialidad integer DEFAULT 1,
    id_titulo integer DEFAULT 1
);


ALTER TABLE public.osiris_his_explfis_parametros OWNER TO admin;

--
-- Name: TABLE osiris_his_explfis_parametros; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_explfis_parametros IS 'Tabla de parametros para la exploracion fisica de las diferentes especialidades';


--
-- Name: COLUMN osiris_his_explfis_parametros.id_tipo_admision; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_explfis_parametros.id_tipo_admision IS 'este campo se enlasa con la tabla de tipos de admisiones departamentos';


--
-- Name: COLUMN osiris_his_explfis_parametros.valor_default; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_explfis_parametros.valor_default IS 'valor defaul para el parametro';


--
-- Name: COLUMN osiris_his_explfis_parametros.id_parametro; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_explfis_parametros.id_parametro IS 'numero para agrupar los parametros';


--
-- Name: COLUMN osiris_his_explfis_parametros.id_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_explfis_parametros.id_especialidad IS 'este campo se enlasa con la tabla de especialidades medicas';


--
-- Name: COLUMN osiris_his_explfis_parametros.id_titulo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_explfis_parametros.id_titulo IS 'este campo se enlasa con los titulos de la exploracion fisica his_explofis_titulos';


--
-- Name: osiris_his_habitaciones; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_habitaciones (
    id_tipo_admisiones integer NOT NULL,
    disponible boolean DEFAULT true NOT NULL,
    descripcion_cuarto character varying,
    id_habitacion integer NOT NULL,
    numero_cuarto integer,
    pid_paciente integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    fecha_de_ocupacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    descripcion_cuarto_corta character varying(4) DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_habitaciones OWNER TO admin;

--
-- Name: TABLE osiris_his_habitaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_habitaciones IS 'tabla que contiene las habitaciones disponibles y no disponibles de hospitalizacion';


--
-- Name: COLUMN osiris_his_habitaciones.id_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_habitaciones.id_tipo_admisiones IS 'area en la que se encuentra la habitacion';


--
-- Name: COLUMN osiris_his_habitaciones.disponible; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_habitaciones.disponible IS 'indica si esta o no ocupada';


--
-- Name: osiris_his_habitaciones_id_habitacion_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_habitaciones_id_habitacion_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_habitaciones_id_habitacion_seq OWNER TO admin;

--
-- Name: osiris_his_habitaciones_id_habitacion_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_habitaciones_id_habitacion_seq OWNED BY osiris_his_habitaciones.id_habitacion;


--
-- Name: osiris_his_historia_clinica; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_historia_clinica (
    id_quien_creo character varying(15),
    id_quien_actualizo character varying(15),
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying(15),
    fhechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fechahora_actualizacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    pid_paciente integer,
    padre_edad integer,
    id_padre_enfermedad integer,
    madre_edad integer,
    id_madre_enfermedad integer,
    hermanos_nrovivos integer,
    hermanos_nromuertos integer,
    id_hermanos_enfermedad integer,
    hijos_nrovivos integer,
    hijos_nromuertos integer,
    id_hijos_enfermedad integer,
    abuelospaternos_nrovivos integer,
    abuelospaternos_nromuertos integer,
    id_abuelospaternos_enfermedad integer,
    abuelosmaternos_nrovivos integer,
    abuelosmaternos_nromuertos integer,
    id_abuelosmaternos_enfermedad integer,
    observaciones_heredo_familiar character varying,
    tipo_casahabitacion character varying,
    no_patologicos_observaciones character varying,
    medicamentos_actuales character varying,
    observaciones_patologicos character varying,
    ginecoobstetricios_menarca integer,
    ginecoobstetricios_ivsa character varying,
    ginecoobstetricios_ritmo character varying,
    ginecoobstetricios_contracepcion character varying,
    ginecoobstetricios_pap character varying,
    ginecoobstetricios_otros character varying,
    ginecoobstetricios_fum timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    ginecoobstetricios_fpp timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    ginecoobstetricios_fup timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    ginecoobstetricios_gestacion integer,
    ginecoobstetricios_parto integer,
    ginecoobstetricios_cesarea integer,
    ginecoobstetricios_aborto integer,
    hcpediatrica_perinatales character varying,
    hcpediatrica_noembarazo integer,
    hcpediatrica_edad_madre integer,
    hcpediatrica_peso integer,
    hcpediatrica_patologicos character varying,
    hcpediatrica_alumbramiento character varying,
    hcpediatrica_edadgestional integer,
    hcpediatrica_infecciones character varying,
    hcpediatrica_alergias character varying,
    hcpediatrica_hospitalizaciones character varying,
    hcpediatrica_traumatismos character varying,
    hcpediatrica_cirugias character varying,
    hcpediatrica_inmunizaciones character varying,
    hcpediatrica_des_psicomotor character varying,
    hcpediatrica_otros character varying,
    motivo_de_ingreso character varying,
    padecimiento_actual character varying,
    psesion_arterial numeric DEFAULT 0,
    frecuencia_cardiaca numeric DEFAULT 0,
    frecuencia_respiratoria numeric DEFAULT 0,
    temperatura numeric DEFAULT 0,
    peso numeric DEFAULT 0,
    talla character varying,
    habitus_exterior character varying,
    cabeza character varying,
    cuello character varying,
    torax character varying,
    abdomen character varying,
    extremidades character varying,
    genitourinario character varying,
    neurologico character varying,
    diagnosticos character varying,
    plan_diagnostico character varying,
    nombre_plan_diag character varying,
    descripcion_enfermedad_padre character varying,
    descripcion_enfermedad_madre character varying,
    descripcion_enfermedad_hermanos character varying,
    descripcion_enfermedad_hijos character varying,
    descripcion_enfermedad_apaternos character varying,
    descripcion_enfermedad_amaternos character varying,
    padre_v_m character varying,
    madre_v_m character varying,
    tabaquismo_p_n character varying,
    alcoholismo_p_n character varying,
    drogas_p_n character varying,
    cronico_degenerativo_p_n character varying,
    hospitalizaciones_p_n character varying,
    quirurgicos_p_n character varying,
    alergicos_p_n character varying,
    traumaticos_p_n character varying,
    neurologicos_p_n character varying,
    historial text,
    id_secuencia integer NOT NULL,
    observaciones_cdegenerativos character varying,
    observaciones_hosp character varying,
    observaciones_quirur character varying,
    observaciones_alergicos character varying,
    observaciones_traumaticos character varying,
    observaciones_neurolog character varying
);


ALTER TABLE public.osiris_his_historia_clinica OWNER TO admin;

--
-- Name: TABLE osiris_his_historia_clinica; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_historia_clinica IS 'se encuentra la informacion de la historia clinica del paciente';


--
-- Name: COLUMN osiris_his_historia_clinica.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_quien_creo IS 'id quien creo la historia del paciente';


--
-- Name: COLUMN osiris_his_historia_clinica.id_quien_actualizo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_quien_actualizo IS 'el id de quien actualizo la historia clinica del paciente';


--
-- Name: COLUMN osiris_his_historia_clinica.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.eliminado IS 'muestra quien elimino la historia clinica';


--
-- Name: COLUMN osiris_his_historia_clinica.id_quien_elimino; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_quien_elimino IS 'el id de la persona que elimino';


--
-- Name: COLUMN osiris_his_historia_clinica.fhechahora_eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.fhechahora_eliminado IS 'muestra la hora y fhecha de la eliminacion';


--
-- Name: COLUMN osiris_his_historia_clinica.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.fechahora_creacion IS 'se muestra la fecha y hora de creacion';


--
-- Name: COLUMN osiris_his_historia_clinica.fechahora_actualizacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.fechahora_actualizacion IS 'la fecha y hora de la actualizacion';


--
-- Name: COLUMN osiris_his_historia_clinica.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.pid_paciente IS 'se encuentra el id del paciente';


--
-- Name: COLUMN osiris_his_historia_clinica.padre_edad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.padre_edad IS 'Antecedentes Heredo Familiar  edad del padre';


--
-- Name: COLUMN osiris_his_historia_clinica.id_padre_enfermedad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_padre_enfermedad IS 'Antecedentes Heredo Familiar  id dela enfermedad del padre';


--
-- Name: COLUMN osiris_his_historia_clinica.madre_edad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.madre_edad IS 'Antecedentes Heredo Familiar muestra la edad de la madre';


--
-- Name: COLUMN osiris_his_historia_clinica.id_madre_enfermedad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_madre_enfermedad IS 'Antecedentes Heredo Familiar id de la enfermedad de la madre';


--
-- Name: COLUMN osiris_his_historia_clinica.hermanos_nrovivos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hermanos_nrovivos IS 'Antecedentes Heredo Familiar el numero de hermanos vivos';


--
-- Name: COLUMN osiris_his_historia_clinica.hermanos_nromuertos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hermanos_nromuertos IS 'Antecedentes Heredo Familiar numero de hermanos muertos';


--
-- Name: COLUMN osiris_his_historia_clinica.id_hermanos_enfermedad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_hermanos_enfermedad IS 'Antecedentes Heredo Familiar se encuentra el id de la enfermedad del hermano';


--
-- Name: COLUMN osiris_his_historia_clinica.hijos_nrovivos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hijos_nrovivos IS 'Antecedentes Heredo Familiar numero de hijos vivos';


--
-- Name: COLUMN osiris_his_historia_clinica.hijos_nromuertos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hijos_nromuertos IS 'Antecedentes Heredo Familiar  numero de hijos muertos';


--
-- Name: COLUMN osiris_his_historia_clinica.id_hijos_enfermedad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_hijos_enfermedad IS '	

Antecedentes Heredo Familiar  id enfermedad de hijos';


--
-- Name: COLUMN osiris_his_historia_clinica.abuelospaternos_nrovivos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.abuelospaternos_nrovivos IS 'Antecedentes Heredo Familiar  abuelos paternos vivos';


--
-- Name: COLUMN osiris_his_historia_clinica.abuelospaternos_nromuertos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.abuelospaternos_nromuertos IS 'Antecedentes Heredo Familiar  abuelos paterno0s muertos';


--
-- Name: COLUMN osiris_his_historia_clinica.id_abuelospaternos_enfermedad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_abuelospaternos_enfermedad IS 'Antecedentes Heredo Familiar  id enfermedad abuelos paternos';


--
-- Name: COLUMN osiris_his_historia_clinica.abuelosmaternos_nrovivos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.abuelosmaternos_nrovivos IS 'Antecedentes Heredo Familiar  abuelos maternos vivos';


--
-- Name: COLUMN osiris_his_historia_clinica.abuelosmaternos_nromuertos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.abuelosmaternos_nromuertos IS 'Antecedentes Heredo Familiar abuelos maternos muertos';


--
-- Name: COLUMN osiris_his_historia_clinica.id_abuelosmaternos_enfermedad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.id_abuelosmaternos_enfermedad IS 'Antecedentes Heredo Familiar Abuelos Maternos Enfermedad';


--
-- Name: COLUMN osiris_his_historia_clinica.observaciones_heredo_familiar; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.observaciones_heredo_familiar IS 'Antecedentes Heredo Familiar  las observaciones(otros) de los antecedentes de heredo familiar';


--
-- Name: COLUMN osiris_his_historia_clinica.tipo_casahabitacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.tipo_casahabitacion IS 'Antecedentes Personales No Patologicos Tipo Casa/Habitacion';


--
-- Name: COLUMN osiris_his_historia_clinica.no_patologicos_observaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.no_patologicos_observaciones IS 'Antecedentes Personales No Patologicos Observaciones';


--
-- Name: COLUMN osiris_his_historia_clinica.medicamentos_actuales; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.medicamentos_actuales IS 'Antecedentes Personales Patologicos Medicamentos Actuales';


--
-- Name: COLUMN osiris_his_historia_clinica.observaciones_patologicos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.observaciones_patologicos IS 'Antecedentes Personales Patologicos Observaciones';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_menarca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_menarca IS '	Antecedentes Gineco Obstetricios Menarca';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_ivsa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_ivsa IS 'Antecedentes Gineco Obstetricios Inicio Vida Sexual Activa';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_ritmo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_ritmo IS 'Antecedentes Gineco Obstetricios Ritmo';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_contracepcion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_contracepcion IS 'Antecedentes Gineco Obstetricios Contracepcion';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_pap; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_pap IS 'Antecedentes Gineco Obstetricios';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_otros; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_otros IS 'Antecedentes Gineco Obstetricios otros';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_fum; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_fum IS 'Antecedentes Gineco Obstetricios Fecha Ultima Menstruacion';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_fpp; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_fpp IS 'Antecedentes Gineco Obstetricios Fecha Probable Parto';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_fup; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_fup IS 'Antecedentes Gineco Obstetricios Fecha Ultimo Parto';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_gestacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_gestacion IS 'Antecedentes Gineco Obstetricios Gestacion';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_parto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_parto IS 'Antecedentes Gineco Obstetricios Parto';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_cesarea; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_cesarea IS 'Antecedentes Gineco Obstetricios Cesarea';


--
-- Name: COLUMN osiris_his_historia_clinica.ginecoobstetricios_aborto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.ginecoobstetricios_aborto IS 'Antecedentes Gineco Obstetricios Abortos';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_perinatales; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_perinatales IS 'Historia Clinica Pediatrica Perinatales';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_noembarazo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_noembarazo IS 'Historia Clinica no embarazo';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_edad_madre; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_edad_madre IS 'Historia Clinica edad madre';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_peso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_peso IS 'Historia Clinica Peso';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_patologicos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_patologicos IS 'Historia Clinica Pediatrica Patologicos';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_alumbramiento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_alumbramiento IS 'Historia Clinica Pediatrica Alumbramiento';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_edadgestional; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_edadgestional IS 'Historia Clinica Pediatrica Edad Gestional';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_infecciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_infecciones IS 'Historia Clinica Pediatrica Infecciones';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_alergias; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_alergias IS 'Historia Clinica Pediatrica Alergias';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_hospitalizaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_hospitalizaciones IS 'Historia Clinica Pediatrica  hospitalizaciones';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_traumatismos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_traumatismos IS 'Historia Clinica Pediatrica Traumatismos';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_cirugias; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_cirugias IS 'Historia Clinica Pediatrica Cirugias';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_inmunizaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_inmunizaciones IS 'Historia Clinica Pediatrica Inmunizaciones';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_des_psicomotor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_des_psicomotor IS 'Historia Clinica Pediatrica Desarrollo Psicomotor';


--
-- Name: COLUMN osiris_his_historia_clinica.hcpediatrica_otros; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hcpediatrica_otros IS 'Historia Clinica Pediatrica (observasiones)';


--
-- Name: COLUMN osiris_his_historia_clinica.motivo_de_ingreso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.motivo_de_ingreso IS 'muestra el motivo de ingreso de la pagina 2';


--
-- Name: COLUMN osiris_his_historia_clinica.padecimiento_actual; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.padecimiento_actual IS 'pagina2: informacion del padecimiento actual';


--
-- Name: COLUMN osiris_his_historia_clinica.psesion_arterial; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.psesion_arterial IS 'pagina2: se encuentra la TA(presion arterial)';


--
-- Name: COLUMN osiris_his_historia_clinica.frecuencia_cardiaca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.frecuencia_cardiaca IS 'Pagina 2: FC(frecuencia cardiaca)';


--
-- Name: COLUMN osiris_his_historia_clinica.frecuencia_respiratoria; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.frecuencia_respiratoria IS 'Pagina 2: FR(frec. respiratoria)';


--
-- Name: COLUMN osiris_his_historia_clinica.temperatura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.temperatura IS 'Pagina2: T(temp.)';


--
-- Name: COLUMN osiris_his_historia_clinica.peso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.peso IS 'Pagina 2: peso';


--
-- Name: COLUMN osiris_his_historia_clinica.talla; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.talla IS 'pagina 2: talla';


--
-- Name: COLUMN osiris_his_historia_clinica.habitus_exterior; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.habitus_exterior IS 'info. del habitus exterior';


--
-- Name: COLUMN osiris_his_historia_clinica.cabeza; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.cabeza IS 'info. de la cabeza';


--
-- Name: COLUMN osiris_his_historia_clinica.cuello; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.cuello IS 'pagina 2:';


--
-- Name: COLUMN osiris_his_historia_clinica.torax; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.torax IS 'pagina 2:';


--
-- Name: COLUMN osiris_his_historia_clinica.abdomen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.abdomen IS 'pagina 2:';


--
-- Name: COLUMN osiris_his_historia_clinica.extremidades; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.extremidades IS 'pagina 2:';


--
-- Name: COLUMN osiris_his_historia_clinica.genitourinario; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.genitourinario IS 'pagina 2:';


--
-- Name: COLUMN osiris_his_historia_clinica.neurologico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.neurologico IS 'pagina 2:';


--
-- Name: COLUMN osiris_his_historia_clinica.diagnosticos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.diagnosticos IS 'pagina 2: info. del diagnostico';


--
-- Name: COLUMN osiris_his_historia_clinica.plan_diagnostico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.plan_diagnostico IS 'pagina 2: info. plan del diagnostico';


--
-- Name: COLUMN osiris_his_historia_clinica.nombre_plan_diag; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.nombre_plan_diag IS 'pagina 2: info. del plan del diag';


--
-- Name: COLUMN osiris_his_historia_clinica.descripcion_enfermedad_padre; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.descripcion_enfermedad_padre IS 'muestra la descripcion de la enfermedad del padre';


--
-- Name: COLUMN osiris_his_historia_clinica.descripcion_enfermedad_madre; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.descripcion_enfermedad_madre IS 'muestra la descripcion de la enfermedad de la madre';


--
-- Name: COLUMN osiris_his_historia_clinica.descripcion_enfermedad_hermanos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.descripcion_enfermedad_hermanos IS 'muestra la descripcion de la enfermedad del hermano(s)';


--
-- Name: COLUMN osiris_his_historia_clinica.descripcion_enfermedad_hijos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.descripcion_enfermedad_hijos IS 'muestra la descripcion de la enfermedad del o los hijos';


--
-- Name: COLUMN osiris_his_historia_clinica.descripcion_enfermedad_apaternos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.descripcion_enfermedad_apaternos IS 'muestra la descripcion de la enfermedad del o los abuelos paternos';


--
-- Name: COLUMN osiris_his_historia_clinica.descripcion_enfermedad_amaternos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.descripcion_enfermedad_amaternos IS 'muestra la descripcion de la enfermedad del o los abuelos maternos';


--
-- Name: COLUMN osiris_his_historia_clinica.padre_v_m; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.padre_v_m IS 'AHF';


--
-- Name: COLUMN osiris_his_historia_clinica.madre_v_m; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.madre_v_m IS 'AHF';


--
-- Name: COLUMN osiris_his_historia_clinica.tabaquismo_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.tabaquismo_p_n IS 'APNP';


--
-- Name: COLUMN osiris_his_historia_clinica.alcoholismo_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.alcoholismo_p_n IS 'APNP';


--
-- Name: COLUMN osiris_his_historia_clinica.drogas_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.drogas_p_n IS 'APNP';


--
-- Name: COLUMN osiris_his_historia_clinica.cronico_degenerativo_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.cronico_degenerativo_p_n IS 'Antecedentes Personales Patologicos';


--
-- Name: COLUMN osiris_his_historia_clinica.hospitalizaciones_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.hospitalizaciones_p_n IS 'APP';


--
-- Name: COLUMN osiris_his_historia_clinica.quirurgicos_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.quirurgicos_p_n IS 'APP';


--
-- Name: COLUMN osiris_his_historia_clinica.alergicos_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.alergicos_p_n IS 'APP';


--
-- Name: COLUMN osiris_his_historia_clinica.traumaticos_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.traumaticos_p_n IS 'APP';


--
-- Name: COLUMN osiris_his_historia_clinica.neurologicos_p_n; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.neurologicos_p_n IS 'APP';


--
-- Name: COLUMN osiris_his_historia_clinica.observaciones_cdegenerativos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_historia_clinica.observaciones_cdegenerativos IS 'observaciones cronicos degenerativos';


--
-- Name: osiris_his_historia_clinica_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_historia_clinica_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_historia_clinica_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_historia_clinica_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_historia_clinica_id_secuencia_seq OWNED BY osiris_his_historia_clinica.id_secuencia;


--
-- Name: osiris_his_historial_habitaciones; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_historial_habitaciones (
    id_quien_asigna character varying(15) DEFAULT 0 NOT NULL,
    folio_de_servicio integer DEFAULT 0 NOT NULL,
    pid_paciente integer DEFAULT 0 NOT NULL,
    fecha_de_asignacion timestamp without time zone DEFAULT '2000-01-01 00:00:00'::timestamp without time zone NOT NULL,
    traspaso boolean DEFAULT false NOT NULL,
    id_habitacion integer DEFAULT 0 NOT NULL,
    id_habitacion_anterior integer DEFAULT 0 NOT NULL,
    dias_de_ocupacion integer DEFAULT 1 NOT NULL,
    id_secuencia integer NOT NULL
);


ALTER TABLE public.osiris_his_historial_habitaciones OWNER TO admin;

--
-- Name: TABLE osiris_his_historial_habitaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_historial_habitaciones IS 'historial de los movimientos en las habitaciones';


--
-- Name: osiris_his_historial_habitaciones_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_historial_habitaciones_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_historial_habitaciones_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_historial_habitaciones_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_historial_habitaciones_id_secuencia_seq OWNED BY osiris_his_historial_habitaciones.id_secuencia;


--
-- Name: osiris_his_hl7; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_hl7 (
    id_secuencia integer NOT NULL,
    pid_paciente integer,
    codigo_hl7_paciente character varying
);


ALTER TABLE public.osiris_his_hl7 OWNER TO admin;

--
-- Name: TABLE osiris_his_hl7; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_hl7 IS 'informacion de HL-7 de pacientes atendidos -Health Level Seven Inc-';


--
-- Name: COLUMN osiris_his_hl7.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_hl7.pid_paciente IS 'almacen el numero de paciente que se enlasa con su historia clinica';


--
-- Name: COLUMN osiris_his_hl7.codigo_hl7_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_hl7.codigo_hl7_paciente IS 'almacen el codigo hl7 que se crea por cada salto del paciente';


--
-- Name: osiris_his_hl7_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_hl7_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_hl7_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_hl7_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_hl7_id_secuencia_seq OWNED BY osiris_his_hl7.id_secuencia;


--
-- Name: osiris_his_informacion_medica; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_informacion_medica (
    id_secuencia integer NOT NULL,
    pid_paciente integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_empleado_creacion character varying DEFAULT ''::bpchar,
    id_medico integer DEFAULT 1,
    notas_de_enfermeria text DEFAULT ''::text,
    notas_de_evolucion text DEFAULT ''::text,
    indicaciones_medicas text DEFAULT ''::text,
    fecha_anotacion date DEFAULT '2000-01-01'::date,
    hora_anotacion character varying(5) DEFAULT ''::bpchar,
    eliminado boolean DEFAULT false
);


ALTER TABLE public.osiris_his_informacion_medica OWNER TO admin;

--
-- Name: TABLE osiris_his_informacion_medica; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_informacion_medica IS 'esta tabla almacena la informacion medica del paciente';


--
-- Name: COLUMN osiris_his_informacion_medica.fecha_anotacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_informacion_medica.fecha_anotacion IS 'almacena la fecha de anotacion';


--
-- Name: osiris_his_medicos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_medicos (
    nombre_medico character varying(60) DEFAULT ''::bpchar,
    telefono1_medico character varying(30) DEFAULT ''::bpchar,
    id_medico integer NOT NULL,
    centro_medico boolean DEFAULT false,
    id_especialidad integer DEFAULT 1,
    telefono2_medico character varying(30) DEFAULT ''::bpchar,
    celular1_medico character varying(35) DEFAULT ''::bpchar,
    celular2_medico character varying(35) DEFAULT ''::bpchar,
    nextel_medico character varying(35) DEFAULT ''::bpchar,
    beeper_medico character varying(35) DEFAULT ''::bpchar,
    id_empresa_medico integer DEFAULT 1,
    titulo_profesional_medico boolean DEFAULT false,
    cedula_profecional_medico boolean DEFAULT false,
    diploma_especialidad_medico boolean DEFAULT false,
    diploma_subespecialidad_medico boolean DEFAULT false,
    copia_identificacion_oficial_medico boolean DEFAULT false,
    copia_cedula_rfc_medico boolean DEFAULT false,
    diploma_cursos_adiestramiento_medico boolean DEFAULT false,
    certificacion_recertificacion_consejo_subespecialidad_medico boolean DEFAULT false,
    copia_comprobante_domicilio_medico boolean DEFAULT false,
    diploma_seminarios_medico boolean DEFAULT false,
    diploma_cursos_medico boolean DEFAULT false,
    diplomas_extranjeros_medico boolean DEFAULT false,
    constancia_congresos_medico boolean DEFAULT false,
    cedula_especialidad_medico boolean DEFAULT false,
    nombre1_medico character varying(50) DEFAULT ''::bpchar,
    nombre2_medico character varying(50) DEFAULT ''::bpchar,
    apellido_paterno_medico character varying(50) DEFAULT ''::bpchar,
    apellido_materno_medico character varying(50) DEFAULT ''::bpchar,
    cedula_medico character varying DEFAULT ''::bpchar,
    fecha_ingreso_medico timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    fecha_revision_medico timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    medico_activo boolean DEFAULT true,
    historial_de_revision text DEFAULT ''::bpchar,
    fecha_autorizacion_medico timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    autorizado boolean DEFAULT false,
    subespecialidad character varying DEFAULT ''::bpchar,
    id_quien_creo_medico character varying DEFAULT ''::bpchar,
    direccion_medico character varying DEFAULT ''::bpchar,
    direccion_consultorio_medico character varying DEFAULT ''::bpchar,
    email_medico character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_medicos OWNER TO admin;

--
-- Name: TABLE osiris_his_medicos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_medicos IS 'Tabla que contiene los nombres de los medicos que laboran en el hospital';


--
-- Name: COLUMN osiris_his_medicos.nombre_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.nombre_medico IS 'nombre del medico';


--
-- Name: COLUMN osiris_his_medicos.telefono1_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.telefono1_medico IS 'telefono del medico';


--
-- Name: COLUMN osiris_his_medicos.id_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.id_especialidad IS 'se enlaza con la tabla de especialidades';


--
-- Name: COLUMN osiris_his_medicos.nombre1_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.nombre1_medico IS 'primer nombre medico';


--
-- Name: COLUMN osiris_his_medicos.nombre2_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.nombre2_medico IS 'segundo y mas nombres de medicos';


--
-- Name: COLUMN osiris_his_medicos.apellido_paterno_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.apellido_paterno_medico IS 'apellido paterno del medico';


--
-- Name: COLUMN osiris_his_medicos.apellido_materno_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.apellido_materno_medico IS 'apellido materno medico';


--
-- Name: COLUMN osiris_his_medicos.cedula_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.cedula_medico IS 'cedula profecional';


--
-- Name: COLUMN osiris_his_medicos.fecha_ingreso_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.fecha_ingreso_medico IS 'fecha de creacion del medico';


--
-- Name: COLUMN osiris_his_medicos.fecha_revision_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.fecha_revision_medico IS 'fecha de revision de medico';


--
-- Name: COLUMN osiris_his_medicos.medico_activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.medico_activo IS 'indica si el medico sigue vigente';


--
-- Name: COLUMN osiris_his_medicos.historial_de_revision; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.historial_de_revision IS 'guarda las revisiones realizadas por el medico encargado';


--
-- Name: COLUMN osiris_his_medicos.fecha_autorizacion_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.fecha_autorizacion_medico IS 'fecha en que el medico se autorizo ';


--
-- Name: COLUMN osiris_his_medicos.autorizado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.autorizado IS 'me indica si el medico fue autorizado';


--
-- Name: COLUMN osiris_his_medicos.id_quien_creo_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_medicos.id_quien_creo_medico IS 'muestra quien creo al medico';


--
-- Name: osiris_his_medicos_id_medico_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_medicos_id_medico_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_medicos_id_medico_seq OWNER TO admin;

--
-- Name: osiris_his_medicos_id_medico_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_medicos_id_medico_seq OWNED BY osiris_his_medicos.id_medico;


--
-- Name: osiris_his_paciente; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_paciente (
    nombre1_paciente character varying(50) DEFAULT ''::character varying NOT NULL,
    nombre2_paciente character varying(50) DEFAULT ''::character varying NOT NULL,
    apellido_paterno_paciente character varying(50) DEFAULT ''::character varying NOT NULL,
    apellido_materno_paciente character varying(50) DEFAULT ''::character varying NOT NULL,
    fecha_nacimiento_paciente date DEFAULT '2000-01-01'::date NOT NULL,
    grupo_sanguineo_paciente character varying(2) DEFAULT ''::bpchar,
    direccion_paciente character varying(60) DEFAULT ''::character varying,
    numero_casa_paciente character varying(10) DEFAULT ''::character varying,
    numero_departamento_paciente character varying(15) DEFAULT ''::character varying,
    codigo_postal_paciente character varying(7) DEFAULT ''::character varying,
    telefono_particular1_paciente character varying(15) DEFAULT ''::character varying,
    telefono_particular2_paciente character varying(35) DEFAULT ''::character varying,
    telefono_trabajo1_paciente character varying(15) DEFAULT ''::character varying,
    telefono_trabajo2_paciente character varying(15) DEFAULT ''::character varying,
    celular1_paciente character varying(15) DEFAULT ''::character varying,
    celular2_paciente character varying(15) DEFAULT ''::character varying,
    fax_paciente character varying(15) DEFAULT ''::character varying,
    email_paciente character varying(60) DEFAULT ''::character varying,
    estado_civil_paciente character varying(25) DEFAULT ''::character varying,
    sexo_paciente character varying(1) DEFAULT ''::bpchar,
    titulo_paciente character varying(6) DEFAULT ''::character varying,
    curp_paciente character varying(20) DEFAULT ''::character varying,
    rfc_paciente character varying(15) DEFAULT ''::bpchar NOT NULL,
    colonia_paciente character varying(50) DEFAULT ''::character varying,
    estado_paciente character varying(30) DEFAULT ''::character varying,
    fecha_muerte_paciente date DEFAULT '2000-01-01'::date,
    causa_muerte_paciente character varying(255) DEFAULT ''::character varying,
    fecha_cambio_info_paciente timestamp with time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    historia_movimientos_paciente text DEFAULT ''::text,
    id_quien_modifico_paciente character varying(7) DEFAULT ''::character varying,
    id_quienlocreo_paciente character varying(15) DEFAULT ''::bpchar NOT NULL,
    pid_paciente integer DEFAULT 0 NOT NULL,
    fechahora_registro_paciente timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    ocupacion_paciente character varying(40) DEFAULT ''::bpchar,
    id_empresa integer DEFAULT 1,
    municipio_paciente character varying(40) DEFAULT ''::bpchar,
    activo boolean DEFAULT true,
    observaciones text DEFAULT ''::bpchar,
    historia_clinica boolean DEFAULT false,
    escolaridad_paciente character varying(40) DEFAULT ''::bpchar,
    religion_paciente character varying(25) DEFAULT ''::bpchar,
    id_linea integer NOT NULL,
    alegias_paciente character varying DEFAULT ''::bpchar,
    lugar_nacimiento_paciente character varying DEFAULT ''::bpchar,
    numero_poliza_sp character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_paciente OWNER TO admin;

--
-- Name: TABLE osiris_his_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_paciente IS 'Almacena toda la informacion de los pacientes que se atienden en el hospital';


--
-- Name: COLUMN osiris_his_paciente.nombre1_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.nombre1_paciente IS 'Son obligatorios';


--
-- Name: COLUMN osiris_his_paciente.nombre2_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.nombre2_paciente IS 'Son obligatorios';


--
-- Name: COLUMN osiris_his_paciente.apellido_paterno_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.apellido_paterno_paciente IS 'Son obligatorios';


--
-- Name: COLUMN osiris_his_paciente.apellido_materno_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.apellido_materno_paciente IS 'Son obligatorios';


--
-- Name: COLUMN osiris_his_paciente.fecha_nacimiento_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.fecha_nacimiento_paciente IS 'Son obligatorios';


--
-- Name: COLUMN osiris_his_paciente.sexo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.sexo_paciente IS 'H=Hombre M=Mujer';


--
-- Name: COLUMN osiris_his_paciente.titulo_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.titulo_paciente IS 'Dr. Ing. Sr. Srta. Sra. CP. Lic.';


--
-- Name: COLUMN osiris_his_paciente.rfc_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.rfc_paciente IS 'aqui se almacena el RFC, del paciente, es obligatorio';


--
-- Name: COLUMN osiris_his_paciente.id_quien_modifico_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.id_quien_modifico_paciente IS 'me indica quien fue el ultimo que modifico la informacion';


--
-- Name: COLUMN osiris_his_paciente.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.pid_paciente IS 'Numero de Expediente del paciente';


--
-- Name: COLUMN osiris_his_paciente.id_empresa; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.id_empresa IS 'es predeterminado en 1 porque se enlasa a la tabla empresas y esta vacio ese campo';


--
-- Name: COLUMN osiris_his_paciente.activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.activo IS 'indica si se puede hacer uso de este pid o no';


--
-- Name: COLUMN osiris_his_paciente.observaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.observaciones IS 'observaciones internas';


--
-- Name: COLUMN osiris_his_paciente.historia_clinica; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.historia_clinica IS 'muestra si el paciente tiene historia clinica';


--
-- Name: COLUMN osiris_his_paciente.escolaridad_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.escolaridad_paciente IS 'almacena la escolaridad que tiene el paciente';


--
-- Name: COLUMN osiris_his_paciente.religion_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.religion_paciente IS 'almacena la religion que tiene el paciente';


--
-- Name: COLUMN osiris_his_paciente.lugar_nacimiento_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_paciente.lugar_nacimiento_paciente IS 'lugar de nacimiento del paciente';


--
-- Name: osiris_his_paciente_id_linea_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_paciente_id_linea_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_paciente_id_linea_seq OWNER TO admin;

--
-- Name: osiris_his_paciente_id_linea_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_paciente_id_linea_seq OWNED BY osiris_his_paciente.id_linea;


--
-- Name: osiris_his_presupuestos_deta; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_presupuestos_deta (
    id_producto numeric(12,0) DEFAULT 1 NOT NULL,
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15),
    id_secuencia integer NOT NULL,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_empleado character varying(15) DEFAULT ''::bpchar,
    cantidad_aplicada numeric(13,6) DEFAULT 0,
    id_tipo_admisiones integer DEFAULT 10,
    id_presupuesto integer DEFAULT 1,
    precio_producto numeric(13,5) DEFAULT 0,
    precio_costo_unitario numeric(13,5) DEFAULT 0,
    porcentage_utilidad numeric(7,3) DEFAULT 0,
    porcentage_iva numeric(5,3) DEFAULT 0
);


ALTER TABLE public.osiris_his_presupuestos_deta OWNER TO admin;

--
-- Name: COLUMN osiris_his_presupuestos_deta.id_presupuesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_deta.id_presupuesto IS 'numero de presupuesto';


--
-- Name: COLUMN osiris_his_presupuestos_deta.precio_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_deta.precio_producto IS 'este campo almacena el precio de producto cuando fue presupuestado';


--
-- Name: COLUMN osiris_his_presupuestos_deta.precio_costo_unitario; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_deta.precio_costo_unitario IS 'almacena el precio de costo unitario';


--
-- Name: COLUMN osiris_his_presupuestos_deta.porcentage_utilidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_deta.porcentage_utilidad IS 'Almacena el porcentage de utilidad aplicado al producto';


--
-- Name: COLUMN osiris_his_presupuestos_deta.porcentage_iva; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_deta.porcentage_iva IS 'Almacena el porcentage del Iva';


--
-- Name: osiris_his_presupuestos_deta_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_presupuestos_deta_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_presupuestos_deta_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_presupuestos_deta_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_presupuestos_deta_id_secuencia_seq OWNED BY osiris_his_presupuestos_deta.id_secuencia;


--
-- Name: osiris_his_presupuestos_enca; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_presupuestos_enca (
    id_presupuesto integer DEFAULT nextval(('public.osiris_his_presupuestos_enca_id_presupuesto_seq'::text)::regclass),
    id_tipo_cirugia integer DEFAULT 1,
    id_medico integer DEFAULT 1,
    total_presupuesto numeric DEFAULT 0.00,
    fecha_de_creacion_presupuesto timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    dias_internamiento integer DEFAULT 0,
    deposito_minimo numeric DEFAULT 0.00,
    precio_convenido numeric(10,2) DEFAULT 0.00,
    id_quien_creo character varying DEFAULT ''::bpchar,
    telefono character varying DEFAULT ''::bpchar,
    fax_presupuesto character varying DEFAULT ''::bpchar,
    telefono_medico character varying DEFAULT ''::bpchar,
    medico_provisional character varying DEFAULT ''::bpchar,
    enviado boolean DEFAULT false,
    notas character varying DEFAULT ''::bpchar,
    cancelado boolean DEFAULT false,
    fechahora_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_cancelo character varying(15) DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_presupuestos_enca OWNER TO admin;

--
-- Name: TABLE osiris_his_presupuestos_enca; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_presupuestos_enca IS 'encabezado de presupuestos';


--
-- Name: COLUMN osiris_his_presupuestos_enca.total_presupuesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.total_presupuesto IS 'total del paquete presupuestado';


--
-- Name: COLUMN osiris_his_presupuestos_enca.deposito_minimo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.deposito_minimo IS 'minimo a pagar';


--
-- Name: COLUMN osiris_his_presupuestos_enca.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.id_quien_creo IS 'login de usuario que creo presupuesto';


--
-- Name: COLUMN osiris_his_presupuestos_enca.telefono; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.telefono IS 'telefono de persona a quien se crea presupuesto';


--
-- Name: COLUMN osiris_his_presupuestos_enca.fax_presupuesto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.fax_presupuesto IS 'fax de la persona';


--
-- Name: COLUMN osiris_his_presupuestos_enca.telefono_medico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.telefono_medico IS 'telefono del medico de presupuesto';


--
-- Name: COLUMN osiris_his_presupuestos_enca.enviado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.enviado IS 'indica si el presupuesto fue enviado a la persona';


--
-- Name: COLUMN osiris_his_presupuestos_enca.notas; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.notas IS 'notas varias';


--
-- Name: COLUMN osiris_his_presupuestos_enca.cancelado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_presupuestos_enca.cancelado IS 'me indica si esta cancelado el presupuesto';


--
-- Name: osiris_his_presupuestos_enca_id_presupuesto_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_presupuestos_enca_id_presupuesto_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_presupuestos_enca_id_presupuesto_seq OWNER TO admin;

--
-- Name: osiris_his_resultados_imagenologia; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_resultados_imagenologia (
    id_producto numeric(12,0),
    folio_imagenologia integer,
    folio_de_servicio integer,
    pid_paciente integer,
    id_secuencia integer NOT NULL
);


ALTER TABLE public.osiris_his_resultados_imagenologia OWNER TO admin;

--
-- Name: TABLE osiris_his_resultados_imagenologia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_resultados_imagenologia IS 'esta tabla almacena los resultados para el area de imagenologia';


--
-- Name: COLUMN osiris_his_resultados_imagenologia.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_imagenologia.id_producto IS 'almacena el codigo de producto se enlasa con la tabla de productos';


--
-- Name: COLUMN osiris_his_resultados_imagenologia.folio_imagenologia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_imagenologia.folio_imagenologia IS 'folio asignado por el departamento de imagenologia';


--
-- Name: COLUMN osiris_his_resultados_imagenologia.folio_de_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_imagenologia.folio_de_servicio IS 'folio asignado y creado en admision';


--
-- Name: COLUMN osiris_his_resultados_imagenologia.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_imagenologia.pid_paciente IS 'pid asignado en admision es su numero de expediente';


--
-- Name: osiris_his_resultados_imagenologia_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_resultados_imagenologia_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_resultados_imagenologia_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_resultados_imagenologia_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_resultados_imagenologia_id_secuencia_seq OWNED BY osiris_his_resultados_imagenologia.id_secuencia;


--
-- Name: osiris_his_resultados_laboratorio; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_resultados_laboratorio (
    id_producto numeric(12,0),
    folio_laboratorio integer,
    folio_de_servicio integer,
    pid_paciente integer,
    parametro character varying(150) DEFAULT ''::bpchar,
    resultado character varying(150) DEFAULT ''::bpchar,
    valor_referencia character varying(150) DEFAULT ''::bpchar,
    id_quien_capturo character varying(15) DEFAULT ''::bpchar,
    fechahora_captura timestamp without time zone DEFAULT '1999-12-31 23:00:00-07'::timestamp with time zone,
    historial_cambios text DEFAULT ''::bpchar,
    id_secuencia integer NOT NULL,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    fechahora_creacion timestamp without time zone DEFAULT '1999-12-31 23:00:00-07'::timestamp with time zone,
    id_quimico character varying DEFAULT ''::bpchar,
    observaciones_de_examen character varying(200) DEFAULT ''::bpchar,
    validado boolean DEFAULT false,
    id_quimico_validacion character varying(15) DEFAULT ''::bpchar,
    fechahora_validacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone
);


ALTER TABLE public.osiris_his_resultados_laboratorio OWNER TO admin;

--
-- Name: TABLE osiris_his_resultados_laboratorio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_resultados_laboratorio IS 'Tabla que guarda los resultados de laboratorio (historial)';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.id_producto IS 'codigo de el producto de el laboratorio';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.folio_laboratorio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.folio_laboratorio IS 'folio consecutivo asignado en el laboratorio';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.folio_de_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.folio_de_servicio IS 'folio asignado en admision';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.pid_paciente IS 'pid asignado en admision ';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.parametro; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.parametro IS 'parametro fijo de el resultado del examen';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.resultado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.resultado IS 'resultado de el examen del laboratorio';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.valor_referencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.valor_referencia IS 'valor de referencia de el examen del laboratorio';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.id_quien_capturo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.id_quien_capturo IS 'id de la persona que capturo';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.fechahora_captura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.fechahora_captura IS 'fecha y hora de captura';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.historial_cambios; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.historial_cambios IS 'historial de cambios';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.id_secuencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.id_secuencia IS 'almacena la secuencia de la tabla';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.id_quien_creo IS 'id de quien creo el examen';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.fechahora_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.fechahora_creacion IS 'fecha y hora de creacion';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.id_quimico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.id_quimico IS 'Numero del quimico que firmara los resultados';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.validado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.validado IS 'bande que indica si el estudio esta validado';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.id_quimico_validacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.id_quimico_validacion IS 'almacena la indentificacion del quimico que valido';


--
-- Name: COLUMN osiris_his_resultados_laboratorio.fechahora_validacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_resultados_laboratorio.fechahora_validacion IS 'almacena la fecha y la hora de cuando se valido el estudio';


--
-- Name: osiris_his_resultados_laboratorio_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_resultados_laboratorio_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_resultados_laboratorio_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_resultados_laboratorio_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_resultados_laboratorio_id_secuencia_seq OWNED BY osiris_his_resultados_laboratorio.id_secuencia;


--
-- Name: osiris_his_solicitudes_deta; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_solicitudes_deta (
    id_secuencia integer NOT NULL,
    folio_de_solicitud integer NOT NULL,
    id_producto numeric(12,0) NOT NULL,
    precio_producto_publico numeric(13,5) DEFAULT 0,
    costo_por_unidad numeric(13,5) DEFAULT 0,
    cantidad_solicitada numeric(8,3) DEFAULT 0,
    cantidad_autorizada numeric(8,3) DEFAULT 0,
    fechahora_solicitud timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_solicito character varying(15) DEFAULT ''::bpchar,
    fechahora_autorizado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_autorizo character varying(15) DEFAULT ''::bpchar,
    status boolean DEFAULT false,
    eliminado boolean DEFAULT false,
    fechahora_elimando timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15) DEFAULT ''::bpchar,
    id_almacen integer,
    surtido boolean DEFAULT false,
    sin_stock boolean DEFAULT false,
    solicitado_erroneo boolean DEFAULT false,
    fecha_envio_almacen timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    envio_directo boolean DEFAULT false,
    id_empleado character varying(15) DEFAULT ''::bpchar,
    id_almacen_origen integer DEFAULT 0,
    traspaso boolean DEFAULT false,
    id_quien_traspaso character varying(15) DEFAULT ''::bpchar,
    fechahora_traspaso timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    numero_de_traspaso integer DEFAULT 0,
    folio_de_servicio integer DEFAULT 0,
    pid_paciente integer DEFAULT 0,
    solicitud_stock boolean DEFAULT false,
    pre_solicitud boolean DEFAULT false,
    nombre_paciente character varying(100) DEFAULT ''::bpchar,
    procedimiento_qx character varying DEFAULT ''::bpchar,
    diagnostico_qx character varying DEFAULT ''::bpchar,
    observaciones_solicitud character varying DEFAULT ''::bpchar,
    tipo_solicitud character varying DEFAULT ''::bpchar,
    autorizada boolean DEFAULT false
);


ALTER TABLE public.osiris_his_solicitudes_deta OWNER TO admin;

--
-- Name: TABLE osiris_his_solicitudes_deta; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_solicitudes_deta IS 'Esta tabla almacena todas la solicitus de materiales que se hacen de los sub-almacenes (hospital, urgencias, etc)';


--
-- Name: COLUMN osiris_his_solicitudes_deta.id_secuencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.id_secuencia IS 'almacen la secuencia';


--
-- Name: COLUMN osiris_his_solicitudes_deta.folio_de_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.folio_de_solicitud IS 'indica el numero de solicituda que se realiza';


--
-- Name: COLUMN osiris_his_solicitudes_deta.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.id_producto IS 'Se enlasa con la tabla de productos';


--
-- Name: COLUMN osiris_his_solicitudes_deta.precio_producto_publico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.precio_producto_publico IS 'Alamacena el precio cuando se hizo el pedido esta es para hacer estadisticas de costeo';


--
-- Name: COLUMN osiris_his_solicitudes_deta.costo_por_unidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.costo_por_unidad IS 'Alamacena el precio de costo para hacer estadisticas de costeo';


--
-- Name: COLUMN osiris_his_solicitudes_deta.cantidad_solicitada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.cantidad_solicitada IS 'alamcena la cantidad solicitada por el sub-alamcen';


--
-- Name: COLUMN osiris_his_solicitudes_deta.cantidad_autorizada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.cantidad_autorizada IS 'este campo se alimenta de almacen';


--
-- Name: COLUMN osiris_his_solicitudes_deta.fechahora_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.fechahora_solicitud IS 'alamcena la fecha de cuando grabo el producto para solicitarlo en almacen';


--
-- Name: COLUMN osiris_his_solicitudes_deta.id_quien_solicito; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.id_quien_solicito IS 'almacena quie hizo la solicitud del producto';


--
-- Name: COLUMN osiris_his_solicitudes_deta.fechahora_autorizado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.fechahora_autorizado IS 'almacena la fecha de autorizacion del producto tambien para el envio directo';


--
-- Name: COLUMN osiris_his_solicitudes_deta.status; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.status IS 'bandera que indica si se ha recibido en almacen y se han realizado cargos';


--
-- Name: COLUMN osiris_his_solicitudes_deta.id_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.id_almacen IS 'se enlasa con la tabla almacenes';


--
-- Name: COLUMN osiris_his_solicitudes_deta.surtido; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.surtido IS 'me indica si este productos  ha sido surtido por almacen';


--
-- Name: COLUMN osiris_his_solicitudes_deta.sin_stock; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.sin_stock IS 'esta campo lo activa el almacenista e indica si se solicito erroneamente';


--
-- Name: COLUMN osiris_his_solicitudes_deta.solicitado_erroneo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.solicitado_erroneo IS 'este campo lo activa el almacenista indicando que ha sido pedido erroneamente';


--
-- Name: COLUMN osiris_his_solicitudes_deta.fecha_envio_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.fecha_envio_almacen IS 'almacena la fecha cuando se envia el pedido al almacen general';


--
-- Name: COLUMN osiris_his_solicitudes_deta.envio_directo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.envio_directo IS 'este campo indica si el producto fue enviado directamente por almacen al sub-almacen';


--
-- Name: COLUMN osiris_his_solicitudes_deta.id_empleado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.id_empleado IS 'almacen al empleado el cual envio la solicitud para almace se elnlasa con tabla osiris_empleados';


--
-- Name: COLUMN osiris_his_solicitudes_deta.id_almacen_origen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.id_almacen_origen IS 'almacen el id del almacen donde sale los productos';


--
-- Name: COLUMN osiris_his_solicitudes_deta.traspaso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.traspaso IS 'indica si esta linea es un traspaso de productos entre almacenes y sub-almacenes';


--
-- Name: COLUMN osiris_his_solicitudes_deta.id_quien_traspaso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.id_quien_traspaso IS 'id de la persona que realizo el traspaso de material';


--
-- Name: COLUMN osiris_his_solicitudes_deta.fechahora_traspaso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.fechahora_traspaso IS 'almcena la fecha de cuando se realiza el traspaso de materiales';


--
-- Name: COLUMN osiris_his_solicitudes_deta.folio_de_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.folio_de_servicio IS 'almacena el folio de servicio del paciente';


--
-- Name: COLUMN osiris_his_solicitudes_deta.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.pid_paciente IS 'almacena el numero de expediente del paciente';


--
-- Name: COLUMN osiris_his_solicitudes_deta.solicitud_stock; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.solicitud_stock IS 'indica que esta solicitud es para alimentar un stock en el subalamacen';


--
-- Name: COLUMN osiris_his_solicitudes_deta.pre_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.pre_solicitud IS 'indica si es una presolicitud de productos para preparar lo necesario en un cirugia';


--
-- Name: COLUMN osiris_his_solicitudes_deta.tipo_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.tipo_solicitud IS 'tipo de requisicion ordinaria o urgente';


--
-- Name: COLUMN osiris_his_solicitudes_deta.autorizada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_deta.autorizada IS 'bandera que indica que autoriza la solicitud';


--
-- Name: osiris_his_solicitudes_deta_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_solicitudes_deta_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_solicitudes_deta_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_solicitudes_deta_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_solicitudes_deta_id_secuencia_seq OWNED BY osiris_his_solicitudes_deta.id_secuencia;


--
-- Name: osiris_his_solicitudes_labrx; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_solicitudes_labrx (
    id_secuencia integer NOT NULL,
    folio_de_solicitud integer,
    folio_de_servicio integer,
    pid_paciente integer,
    id_producto numeric(12,0),
    precio_producto_publico numeric(13,5) DEFAULT 0,
    costo_por_unidad numeric(13,5) DEFAULT 0,
    cantidad_solicitada numeric(8,3) DEFAULT 0,
    cantidad_autorizada numeric(8,3) DEFAULT 0,
    fechahora_solicitud timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_solicito character varying(15) DEFAULT ''::bpchar,
    fechahora_autorizado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_autorizo character varying(15) DEFAULT ''::bpchar,
    status boolean DEFAULT false,
    id_proveedor integer DEFAULT 1,
    id_tipo_admisiones integer DEFAULT 0,
    id_tipo_admisiones2 integer DEFAULT 0,
    folio_interno_labrx integer DEFAULT 0,
    area_quien_solicita character varying DEFAULT ''::bpchar,
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15) DEFAULT ''::bpchar,
    observaciones_solicitud text DEFAULT ''::bpchar,
    turno character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_solicitudes_labrx OWNER TO admin;

--
-- Name: TABLE osiris_his_solicitudes_labrx; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_solicitudes_labrx IS 'esta tabla almacena las solicitudes de laboratorio y rayos x';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.id_secuencia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.id_secuencia IS 'almacen la secuencia';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.folio_de_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.folio_de_solicitud IS 'indica el numero de solicituda que se realiza';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.folio_de_servicio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.folio_de_servicio IS 'almacena el folio de servicio del paciente';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.pid_paciente; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.pid_paciente IS 'alamcena el numero de expediente del palciente';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.id_producto IS 'Se enlasa con la tabla de productos';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.precio_producto_publico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.precio_producto_publico IS '	Alamacena el precio cuando se hizo el pedido esta es para hacer estadisticas de costeo';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.costo_por_unidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.costo_por_unidad IS 'Alamacena el precio de costo para hacer estadisticas de costeo';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.cantidad_solicitada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.cantidad_solicitada IS 'almacena la cantidad solicitada por el area';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.cantidad_autorizada; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.cantidad_autorizada IS 'este campo se alimenta por parte del area';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.fechahora_solicitud; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.fechahora_solicitud IS 'alamcena la fecha de cuando grabo el producto para solicitarlo';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.id_quien_solicito; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.id_quien_solicito IS '	almacena quie hizo la solicitud del producto';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.fechahora_autorizado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.fechahora_autorizado IS 'almacena la fecha de autorizacion del producto tambien para el envio directo';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.status; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.status IS 'indica si esta solicitud fue cargada al paciente';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.id_proveedor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.id_proveedor IS 'este campo se enlasa con la tabla de proveedores';


--
-- Name: COLUMN osiris_his_solicitudes_labrx.turno; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_solicitudes_labrx.turno IS 'almacena el turno de la solicitud';


--
-- Name: osiris_his_solicitudes_labrx_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_solicitudes_labrx_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_solicitudes_labrx_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_solicitudes_labrx_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_solicitudes_labrx_id_secuencia_seq OWNED BY osiris_his_solicitudes_labrx.id_secuencia;


--
-- Name: osiris_his_somatometria; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_somatometria (
    id_secuencia integer NOT NULL,
    folio_de_servicio integer DEFAULT 0,
    pid_paciente integer DEFAULT 0,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_empleado_creacion character varying DEFAULT ''::bpchar,
    fecha_somatometria date DEFAULT '2000-01-01'::date,
    hora_somatometria character varying(5) DEFAULT '00:00'::character varying,
    cancelado boolean DEFAULT false,
    fechahora_cancelacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_cancelo character varying DEFAULT ''::bpchar,
    tension_arterial character varying DEFAULT ''::bpchar,
    pulso numeric DEFAULT 0,
    frecuencia_respiratoria numeric(7,2) DEFAULT 0,
    temperatura numeric(7,2) DEFAULT 0,
    saturacion_oxigeno character varying DEFAULT ''::bpchar,
    diuresis character varying DEFAULT ''::bpchar,
    evacuacion character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_somatometria OWNER TO admin;

--
-- Name: COLUMN osiris_his_somatometria.id_empleado_creacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.id_empleado_creacion IS 'alamacen el id del empleado quien captura la somatometria';


--
-- Name: COLUMN osiris_his_somatometria.fecha_somatometria; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.fecha_somatometria IS 'fecha en la cual se ha tomado la somatometria';


--
-- Name: COLUMN osiris_his_somatometria.hora_somatometria; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.hora_somatometria IS 'hora de cuando se tomo la somatometria';


--
-- Name: COLUMN osiris_his_somatometria.cancelado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.cancelado IS 'bandera que indica si esta camcelada este registro de somatometria';


--
-- Name: COLUMN osiris_his_somatometria.fechahora_cancelacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.fechahora_cancelacion IS 'almacena la hora y la fecha de cuando se cancelo la somatometria';


--
-- Name: COLUMN osiris_his_somatometria.id_quien_cancelo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.id_quien_cancelo IS 'almacena el id de quien cancelo la somatometria';


--
-- Name: COLUMN osiris_his_somatometria.tension_arterial; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.tension_arterial IS 'almacena el valor de la tension arterial';


--
-- Name: COLUMN osiris_his_somatometria.pulso; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.pulso IS 'almacena el valor del pulso del paciente';


--
-- Name: COLUMN osiris_his_somatometria.frecuencia_respiratoria; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.frecuencia_respiratoria IS 'almacena la frecuencia respiratoria del paciente';


--
-- Name: COLUMN osiris_his_somatometria.temperatura; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.temperatura IS 'almacena la temperatura del paciente';


--
-- Name: COLUMN osiris_his_somatometria.saturacion_oxigeno; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_somatometria.saturacion_oxigeno IS 'almacena los parametros de saturacion del oxigeno';


--
-- Name: osiris_his_somatometria_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_somatometria_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_somatometria_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_his_somatometria_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_somatometria_id_secuencia_seq OWNED BY osiris_his_somatometria.id_secuencia;


--
-- Name: osiris_his_tipo_admisiones; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_tipo_admisiones (
    descripcion_admisiones character varying(40) DEFAULT ''::bpchar,
    id_tipo_admisiones integer NOT NULL,
    servicio_directo boolean DEFAULT true,
    cuenta_mayor integer,
    grupo character varying(3) DEFAULT ''::character varying,
    habitaciones boolean DEFAULT false,
    sub_almacen boolean DEFAULT false,
    nutricion_cafeteria boolean DEFAULT false,
    cuenta_mayor_egreso integer,
    agrupacion character varying(3) DEFAULT ''::bpchar,
    puede_requisar boolean DEFAULT false,
    activo_admision boolean DEFAULT true,
    activo_caja boolean DEFAULT false,
    asigna_med_trantante boolean DEFAULT false,
    asigna_cirugia boolean DEFAULT false,
    activo_valid_prodrequi boolean DEFAULT false,
    mail_centro_costo character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_his_tipo_admisiones OWNER TO admin;

--
-- Name: TABLE osiris_his_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_tipo_admisiones IS 'Almacena todos los tipos de admision que actualmente el hospital tiene';


--
-- Name: COLUMN osiris_his_tipo_admisiones.servicio_directo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.servicio_directo IS 'Especifica si el servicio es directo o  para pago en caja (true) o es de internamiento (false)';


--
-- Name: COLUMN osiris_his_tipo_admisiones.cuenta_mayor; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.cuenta_mayor IS 'especifica la cuenta mayor que esta en el plan de cuenta contable';


--
-- Name: COLUMN osiris_his_tipo_admisiones.grupo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.grupo IS 'Señala el tipo de departamento ADMinistrativo o MEDico';


--
-- Name: COLUMN osiris_his_tipo_admisiones.habitaciones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.habitaciones IS 'indica si esta area cuenta con habitaciones o cubiculos';


--
-- Name: COLUMN osiris_his_tipo_admisiones.sub_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.sub_almacen IS 'este campo indica si este centro de costo cuenta con un sub-almacen';


--
-- Name: COLUMN osiris_his_tipo_admisiones.nutricion_cafeteria; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.nutricion_cafeteria IS 'especifica si este campo se puede leer para nutricion y cafeteria';


--
-- Name: COLUMN osiris_his_tipo_admisiones.agrupacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.agrupacion IS 'este campo se enlasa con la tabla de agrupacion de produtos';


--
-- Name: COLUMN osiris_his_tipo_admisiones.puede_requisar; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.puede_requisar IS 'esta bandera indica si el centro de costo puede realizar requisiciones';


--
-- Name: COLUMN osiris_his_tipo_admisiones.activo_caja; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.activo_caja IS 'Activa el acceso para el modulo de caja';


--
-- Name: COLUMN osiris_his_tipo_admisiones.asigna_med_trantante; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.asigna_med_trantante IS 'bandera que indica si el tipo de admision es obligatorio la asignacion del medico tratante';


--
-- Name: COLUMN osiris_his_tipo_admisiones.asigna_cirugia; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.asigna_cirugia IS 'bandera que indica si el tipo de admision es obligatorio la asignacion de la cirugia';


--
-- Name: COLUMN osiris_his_tipo_admisiones.activo_valid_prodrequi; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.activo_valid_prodrequi IS 'bandera que indica que el departamento verifica si existe un producto requisado y no ha sido comprado aun';


--
-- Name: COLUMN osiris_his_tipo_admisiones.mail_centro_costo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_admisiones.mail_centro_costo IS 'almacen el correo electronico del departamento para ligarlo a servidor de correo';


--
-- Name: osiris_his_tipo_cirugias; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_tipo_cirugias (
    id_tipo_cirugia integer NOT NULL,
    descripcion_cirugia character varying(255),
    tiene_paquete boolean DEFAULT false,
    id_especialidad integer DEFAULT 1,
    dias_internamiento integer DEFAULT 0,
    valor_paquete numeric DEFAULT 0.00,
    deposito_minimo numeric DEFAULT 0.00,
    precio_de_venta numeric(10,2) DEFAULT 0.00,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    fechahora_creacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    eliminado boolean DEFAULT false,
    id_quien_elimino character varying(15),
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    paquete_checkup boolean DEFAULT false
);


ALTER TABLE public.osiris_his_tipo_cirugias OWNER TO admin;

--
-- Name: TABLE osiris_his_tipo_cirugias; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_tipo_cirugias IS 'esta tabla almacena todos los tipos de cirugia que realiza el hospital';


--
-- Name: COLUMN osiris_his_tipo_cirugias.tiene_paquete; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_cirugias.tiene_paquete IS 'la bandera se enciende si tiene paquete';


--
-- Name: COLUMN osiris_his_tipo_cirugias.id_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_cirugias.id_especialidad IS 'este campo se enlasa con la tabla de especialidades medicas';


--
-- Name: COLUMN osiris_his_tipo_cirugias.dias_internamiento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_cirugias.dias_internamiento IS 'indica los dias de internamiento de cada cirugia se aplica para el paquete';


--
-- Name: COLUMN osiris_his_tipo_cirugias.valor_paquete; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_cirugias.valor_paquete IS 'valor total del paquete';


--
-- Name: COLUMN osiris_his_tipo_cirugias.deposito_minimo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_cirugias.deposito_minimo IS 'deposito para separar paquete';


--
-- Name: COLUMN osiris_his_tipo_cirugias.paquete_checkup; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_cirugias.paquete_checkup IS 'indica si es un servicio de checkup';


--
-- Name: osiris_his_tipo_cirugias_id_tipo_cirugia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_tipo_cirugias_id_tipo_cirugia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_tipo_cirugias_id_tipo_cirugia_seq OWNER TO admin;

--
-- Name: osiris_his_tipo_cirugias_id_tipo_cirugia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_tipo_cirugias_id_tipo_cirugia_seq OWNED BY osiris_his_tipo_cirugias.id_tipo_cirugia;


--
-- Name: osiris_his_tipo_diagnosticos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_tipo_diagnosticos (
    descripcion_diagnostico character varying(255),
    id_especialidad integer DEFAULT 1,
    id_diagnostico integer NOT NULL,
    id_cie_10 character varying DEFAULT ''::bpchar,
    id_cie_10_grupo integer DEFAULT 0,
    sub_grupo boolean DEFAULT false
);


ALTER TABLE public.osiris_his_tipo_diagnosticos OWNER TO admin;

--
-- Name: TABLE osiris_his_tipo_diagnosticos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_tipo_diagnosticos IS 'Tabal de diagnosticos';


--
-- Name: COLUMN osiris_his_tipo_diagnosticos.descripcion_diagnostico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_diagnosticos.descripcion_diagnostico IS 'diagnostico dado por doctor';


--
-- Name: COLUMN osiris_his_tipo_diagnosticos.id_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_diagnosticos.id_especialidad IS 'este campo se enlasa con la tabla de especialidades';


--
-- Name: COLUMN osiris_his_tipo_diagnosticos.id_diagnostico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_diagnosticos.id_diagnostico IS 'codigo interno del hospital para los tipos de diagnostico';


--
-- Name: COLUMN osiris_his_tipo_diagnosticos.id_cie_10; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_diagnosticos.id_cie_10 IS 'almacena el codifo realacionado al la norma internacional CIE-10';


--
-- Name: COLUMN osiris_his_tipo_diagnosticos.id_cie_10_grupo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_diagnosticos.id_cie_10_grupo IS 'almacena el id del capitulo del CIE-10';


--
-- Name: COLUMN osiris_his_tipo_diagnosticos.sub_grupo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_diagnosticos.sub_grupo IS 'valida los titulos de los grupos de cada enfermedad';


--
-- Name: osiris_his_tipo_diagnosticos_id_diagnostico_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_tipo_diagnosticos_id_diagnostico_seq
    START WITH 14196
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_tipo_diagnosticos_id_diagnostico_seq OWNER TO admin;

--
-- Name: osiris_his_tipo_diagnosticos_id_diagnostico_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_tipo_diagnosticos_id_diagnostico_seq OWNED BY osiris_his_tipo_diagnosticos.id_diagnostico;


--
-- Name: osiris_his_tipo_enfermedades; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_tipo_enfermedades (
    descripcion_enfermedad integer NOT NULL,
    id_enfermedad integer NOT NULL
);


ALTER TABLE public.osiris_his_tipo_enfermedades OWNER TO admin;

--
-- Name: TABLE osiris_his_tipo_enfermedades; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_tipo_enfermedades IS 'en esta tabla puede encontrar el tipo de enfermedades';


--
-- Name: COLUMN osiris_his_tipo_enfermedades.descripcion_enfermedad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_enfermedades.descripcion_enfermedad IS 'la descripcion o tipo de enfermedad';


--
-- Name: osiris_his_tipo_enfermedades_id_enfermedad_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_tipo_enfermedades_id_enfermedad_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_tipo_enfermedades_id_enfermedad_seq OWNER TO admin;

--
-- Name: osiris_his_tipo_enfermedades_id_enfermedad_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_tipo_enfermedades_id_enfermedad_seq OWNED BY osiris_his_tipo_enfermedades.id_enfermedad;


--
-- Name: osiris_his_tipo_especialidad; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_tipo_especialidad (
    descripcion_especialidad character varying(60),
    id_especialidad integer NOT NULL,
    sub_especialidad boolean DEFAULT false,
    osiris_his_tipo_admisiones numeric DEFAULT 1,
    espcialidad_activo boolean DEFAULT false
);


ALTER TABLE public.osiris_his_tipo_especialidad OWNER TO admin;

--
-- Name: TABLE osiris_his_tipo_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_tipo_especialidad IS 'Tabla que muestra las especialidades de la medicina';


--
-- Name: COLUMN osiris_his_tipo_especialidad.descripcion_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_especialidad.descripcion_especialidad IS 'nombre de la especialidad';


--
-- Name: COLUMN osiris_his_tipo_especialidad.sub_especialidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_especialidad.sub_especialidad IS 'indica si es una sub-especilidad';


--
-- Name: COLUMN osiris_his_tipo_especialidad.osiris_his_tipo_admisiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_especialidad.osiris_his_tipo_admisiones IS 'campo de enlase con los tipos de admisiones para consulta medica';


--
-- Name: osiris_his_tipo_especialidad_id_especialidad_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_tipo_especialidad_id_especialidad_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_tipo_especialidad_id_especialidad_seq OWNER TO admin;

--
-- Name: osiris_his_tipo_especialidad_id_especialidad_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_tipo_especialidad_id_especialidad_seq OWNED BY osiris_his_tipo_especialidad.id_especialidad;


--
-- Name: osiris_his_tipo_pacientes; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_his_tipo_pacientes (
    descripcion_tipo_paciente character varying(40),
    id_tipo_paciente integer NOT NULL,
    lista_de_precio boolean DEFAULT false,
    id_tipo_documento integer DEFAULT 1
);


ALTER TABLE public.osiris_his_tipo_pacientes OWNER TO admin;

--
-- Name: TABLE osiris_his_tipo_pacientes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_his_tipo_pacientes IS 'Almacena los tipos de paciente que se atienden en el hospital';


--
-- Name: COLUMN osiris_his_tipo_pacientes.lista_de_precio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_pacientes.lista_de_precio IS 'Este campo me indica si tiene lista de precio asociada con aseguradora o empresa';


--
-- Name: COLUMN osiris_his_tipo_pacientes.id_tipo_documento; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_his_tipo_pacientes.id_tipo_documento IS 'este campo se enlasa con la tabla de tipos de documentos';


--
-- Name: osiris_his_tipo_pacientes_id_tipo_paciente_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_his_tipo_pacientes_id_tipo_paciente_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_his_tipo_pacientes_id_tipo_paciente_seq OWNER TO admin;

--
-- Name: osiris_his_tipo_pacientes_id_tipo_paciente_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_his_tipo_pacientes_id_tipo_paciente_seq OWNED BY osiris_his_tipo_pacientes.id_tipo_paciente;


--
-- Name: osiris_information_medical_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_information_medical_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_information_medical_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_information_medical_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_information_medical_id_secuencia_seq OWNED BY osiris_his_informacion_medica.id_secuencia;


--
-- Name: osiris_inventario_almacenes; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_inventario_almacenes (
    id_producto numeric(12,0),
    id_almacen integer,
    stock numeric(10,3) DEFAULT 0,
    fechahora_alta timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_creo character varying(15) DEFAULT ''::bpchar,
    ano_inventario numeric(4,0),
    mes_inventario numeric(2,0),
    id_secuencia integer NOT NULL,
    eliminado boolean DEFAULT false,
    fechahora_eliminado timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_elimino character varying(15) DEFAULT ''::bpchar,
    lote character varying DEFAULT ''::bpchar,
    caducidad character varying DEFAULT ''::bpchar,
    codigo_barras character varying DEFAULT ''::bpchar,
    ajuste_inventario boolean DEFAULT false,
    fechohora_ajuste_inventario timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_ajusto character varying DEFAULT ''::bpchar
);


ALTER TABLE public.osiris_inventario_almacenes OWNER TO admin;

--
-- Name: TABLE osiris_inventario_almacenes; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_inventario_almacenes IS 'Tabla que contiene informacion de la tabla catalogo almacenes ';


--
-- Name: COLUMN osiris_inventario_almacenes.id_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.id_producto IS 'este campo se enlasa con la tabla de producos';


--
-- Name: COLUMN osiris_inventario_almacenes.id_almacen; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.id_almacen IS 'este campo se enlasa con la tabla de almacenes';


--
-- Name: COLUMN osiris_inventario_almacenes.stock; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.stock IS 'este campo almacena la captura del producto x usuario no totaliza por productos';


--
-- Name: COLUMN osiris_inventario_almacenes.id_quien_creo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.id_quien_creo IS 'almacena quien hizo la alta';


--
-- Name: COLUMN osiris_inventario_almacenes.ano_inventario; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.ano_inventario IS 'almacena el año del inventario';


--
-- Name: COLUMN osiris_inventario_almacenes.mes_inventario; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.mes_inventario IS 'almacena en mes del inventario ';


--
-- Name: COLUMN osiris_inventario_almacenes.eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.eliminado IS 'indica si esta eliminado';


--
-- Name: COLUMN osiris_inventario_almacenes.fechahora_eliminado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.fechahora_eliminado IS 'almacena la fecha y la hora de eliminacion';


--
-- Name: COLUMN osiris_inventario_almacenes.id_quien_elimino; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.id_quien_elimino IS 'almacena al personal autorizado de la eliminacion';


--
-- Name: COLUMN osiris_inventario_almacenes.lote; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.lote IS 'almacena el numero o serie del lote';


--
-- Name: COLUMN osiris_inventario_almacenes.caducidad; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.caducidad IS 'almacena la caducidad del producto';


--
-- Name: COLUMN osiris_inventario_almacenes.codigo_barras; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.codigo_barras IS 'almacena el codigo de barras del producto';


--
-- Name: COLUMN osiris_inventario_almacenes.ajuste_inventario; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.ajuste_inventario IS 'bandera que indica si el producto se ajusto al inventario general en almacen general';


--
-- Name: COLUMN osiris_inventario_almacenes.fechohora_ajuste_inventario; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_inventario_almacenes.fechohora_ajuste_inventario IS 'almacena la fecha y la hora cuando se realizo el ajuste a inventario del almacen general';


--
-- Name: osiris_inventario_almacenes_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_inventario_almacenes_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_inventario_almacenes_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_inventario_almacenes_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_inventario_almacenes_id_secuencia_seq OWNED BY osiris_inventario_almacenes.id_secuencia;


--
-- Name: osiris_municipios; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_municipios (
    descripcion_municipio character varying,
    id_municipio integer DEFAULT nextval(('public.osiris_municipios_id_secuencia_seq'::text)::regclass),
    id_estado integer
);


ALTER TABLE public.osiris_municipios OWNER TO admin;

--
-- Name: TABLE osiris_municipios; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_municipios IS 'tabla que contiene los municipios del estado y/o pais';


--
-- Name: COLUMN osiris_municipios.descripcion_municipio; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_municipios.descripcion_municipio IS 'descrpcion del municipio';


--
-- Name: COLUMN osiris_municipios.id_estado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_municipios.id_estado IS 'se enlaza con la tabla de estados';


--
-- Name: osiris_municipios_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_municipios_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_municipios_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_productos; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_productos (
    id_producto numeric(12,0),
    descripcion_producto character varying,
    id_grupo_producto integer DEFAULT 0,
    id_grupo1_producto integer DEFAULT 0,
    id_grupo2_producto integer DEFAULT 0,
    precio_producto_publico numeric(13,5) DEFAULT 0,
    costo_producto numeric(13,5) DEFAULT 0,
    costo_por_unidad numeric(13,5) DEFAULT 0,
    porcentage_ganancia numeric(7,3) DEFAULT 0,
    id_quienlocreo_producto character varying(15),
    fechahora_creacion timestamp without time zone DEFAULT '1999-12-31 23:00:00-07'::timestamp with time zone,
    aplicar_iva boolean DEFAULT true,
    aplica_descuento boolean DEFAULT false,
    cobro_activo boolean DEFAULT true,
    tiene_kit boolean DEFAULT false,
    nombre_articulo character varying,
    nombre_generico_articulo character varying,
    cantidad_de_embalaje numeric(6,2),
    porcentage_descuento numeric(6,3),
    costo_unico boolean DEFAULT false,
    historial_de_cambios text DEFAULT ''::bpchar,
    precio_producto_publico1 numeric(13,5) DEFAULT 0,
    precio_producto_5003 numeric(13,5) DEFAULT 0,
    precio_producto_1028 numeric(13,5) DEFAULT 0,
    cargo_extra_aseguradora boolean DEFAULT false,
    producto_validado boolean DEFAULT false,
    fechahora_validacion timestamp without time zone DEFAULT '2000-01-02 00:00:00'::timestamp without time zone,
    id_quien_valido character varying(15) DEFAULT ''::bpchar,
    tipo_unidad_producto character varying(15) DEFAULT ''::bpchar,
    precio_producto_50016 numeric(13,5) DEFAULT 0,
    id_secuencia integer NOT NULL,
    precio_producto_700 numeric(13,5) DEFAULT 0,
    precio_producto_50029 numeric(13,5) DEFAULT 0
);


ALTER TABLE public.osiris_productos OWNER TO admin;

--
-- Name: TABLE osiris_productos; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_productos IS 'Tabla general de productos';


--
-- Name: COLUMN osiris_productos.descripcion_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.descripcion_producto IS 'nombre como lo describe el hispotal para su uso';


--
-- Name: COLUMN osiris_productos.id_grupo_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.id_grupo_producto IS 'Este campo se enlasa con la tabla grupo_producto';


--
-- Name: COLUMN osiris_productos.id_grupo1_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.id_grupo1_producto IS 'Este campo se enlasa a la tabla sub_grupo1';


--
-- Name: COLUMN osiris_productos.id_grupo2_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.id_grupo2_producto IS 'este campo se enlasa con la tabla sub_grupo2 para el manejo de productos';


--
-- Name: COLUMN osiris_productos.precio_producto_publico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.precio_producto_publico IS 'precio a cobrar a publico';


--
-- Name: COLUMN osiris_productos.cobro_activo; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.cobro_activo IS 'me indica si este producto esta activado para que se pueda cargar';


--
-- Name: COLUMN osiris_productos.tiene_kit; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.tiene_kit IS 'me indica la bandera que es armado mediante kit';


--
-- Name: COLUMN osiris_productos.cantidad_de_embalaje; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.cantidad_de_embalaje IS 'indica la cantidad de unidades del producto';


--
-- Name: COLUMN osiris_productos.costo_unico; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.costo_unico IS 'esta varibale me indica si este producto va ser de costo unico, se utiliza para calcular los usos';


--
-- Name: COLUMN osiris_productos.precio_producto_publico1; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.precio_producto_publico1 IS 'este campo sirve para dar otro precio a un cliente o varios clientes si esta en cero se aplica precio_producto_publico';


--
-- Name: COLUMN osiris_productos.precio_producto_5003; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.precio_producto_5003 IS 'este campo me indica el precio a la tipo de admision 500 y la empresa 1';


--
-- Name: COLUMN osiris_productos.precio_producto_1028; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.precio_producto_1028 IS 'almacen productos con tabla tipo paciente y tipo de empresa';


--
-- Name: COLUMN osiris_productos.cargo_extra_aseguradora; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.cargo_extra_aseguradora IS 'indica si el producto a carga es un extra cuando es una seguradora';


--
-- Name: COLUMN osiris_productos.producto_validado; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.producto_validado IS 'esta validacion sirve cuando el producto se ingreso para crear una requisicion';


--
-- Name: COLUMN osiris_productos.fechahora_validacion; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.fechahora_validacion IS 'almacen la fecha y la hora cuando se valido el producto';


--
-- Name: COLUMN osiris_productos.id_quien_valido; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.id_quien_valido IS 'almacen quien valido el producto';


--
-- Name: COLUMN osiris_productos.tipo_unidad_producto; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.tipo_unidad_producto IS 'alamcena el tipo de unidad del producto';


--
-- Name: COLUMN osiris_productos.precio_producto_50016; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.precio_producto_50016 IS 'precio municipio santa catarina';


--
-- Name: COLUMN osiris_productos.precio_producto_700; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.precio_producto_700 IS 'precio para seccion 50';


--
-- Name: COLUMN osiris_productos.precio_producto_50029; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON COLUMN osiris_productos.precio_producto_50029 IS 'lista de precios Municipio de San Nicolas';


--
-- Name: osiris_productos_id_secuencia_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_productos_id_secuencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_productos_id_secuencia_seq OWNER TO admin;

--
-- Name: osiris_productos_id_secuencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_productos_id_secuencia_seq OWNED BY osiris_productos.id_secuencia;


--
-- Name: osiris_secuencia_deta_factura_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_secuencia_deta_factura_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_secuencia_deta_factura_seq OWNER TO admin;

--
-- Name: osiris_tipos_religiones; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE osiris_tipos_religiones (
    id_tipo_religion integer NOT NULL,
    descripcion_religion character varying DEFAULT ''::bpchar,
    activo boolean DEFAULT true
);


ALTER TABLE public.osiris_tipos_religiones OWNER TO admin;

--
-- Name: TABLE osiris_tipos_religiones; Type: COMMENT; Schema: public; Owner: admin
--

COMMENT ON TABLE osiris_tipos_religiones IS 'Almacena los tipos de religiones';


--
-- Name: osiris_tipos_religiones_id_tipo_religion_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE osiris_tipos_religiones_id_tipo_religion_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.osiris_tipos_religiones_id_tipo_religion_seq OWNER TO admin;

--
-- Name: osiris_tipos_religiones_id_tipo_religion_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE osiris_tipos_religiones_id_tipo_religion_seq OWNED BY osiris_tipos_religiones.id_tipo_religion;


--
-- Name: seguro_popular; Type: TABLE; Schema: public; Owner: admin; Tablespace: 
--

CREATE TABLE seguro_popular (
    id_secuencia integer NOT NULL,
    nombre_paciente character varying DEFAULT ''::bpchar,
    sexo_paciente character varying DEFAULT ''::bpchar,
    edad_paciente integer DEFAULT 0,
    declaratoria integer,
    numero_poliza character varying DEFAULT ''::bpchar,
    pid_paciente integer,
    factura character varying,
    fecha_diagnostico date,
    patologia character varying DEFAULT ''::bpchar,
    fecha_de_atencion date,
    fase_atencion character varying DEFAULT ''::bpchar,
    tipo_fase_atencion character varying DEFAULT ''::bpchar,
    organo character varying DEFAULT ''::bpchar,
    etapa character varying DEFAULT ''::bpchar,
    grupo character varying DEFAULT ''::bpchar,
    tipo_caso character varying DEFAULT ''::bpchar,
    id_secuencia2 integer
);


ALTER TABLE public.seguro_popular OWNER TO admin;

--
-- Name: seguro_popular_id_secuencis_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

CREATE SEQUENCE seguro_popular_id_secuencis_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


ALTER TABLE public.seguro_popular_id_secuencis_seq OWNER TO admin;

--
-- Name: seguro_popular_id_secuencis_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: admin
--

ALTER SEQUENCE seguro_popular_id_secuencis_seq OWNED BY seguro_popular.id_secuencia;


--
-- Name: id_aseguradora; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_aseguradoras ALTER COLUMN id_aseguradora SET DEFAULT nextval('osiris_aseguradoras_id_aseguradora_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_catalogo_almacenes ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_catalogo_almacenes_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_catalogo_productos_proveedores ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_catalogo_productos_proveedores_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_empleado_detalle ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_empleado_detalle_id_secuencia_seq'::regclass);


--
-- Name: id; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_empleados_accesos ALTER COLUMN id SET DEFAULT nextval('osiris_empleados_accesos_id_seq'::regclass);


--
-- Name: id_empresa; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_empresas ALTER COLUMN id_empresa SET DEFAULT nextval('osiris_empresas_id_empresa_seq'::regclass);


--
-- Name: id_abono; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_abonos ALTER COLUMN id_abono SET DEFAULT nextval('osiris_erp_abonos_id_abono_seq'::regclass);


--
-- Name: id_cliente; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_clientes ALTER COLUMN id_cliente SET DEFAULT nextval('osiris_erp_clientes_id_cliente_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_cobros_deta ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_cobros_deta_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_codigos_barra ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_codigos_barra_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_compra_farmacia ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_compra_farmacia_id_secuencia_seq'::regclass);


--
-- Name: id_comprobante_pagare; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_comprobante_pagare ALTER COLUMN id_comprobante_pagare SET DEFAULT nextval('osiris_erp_comprobante_pagare_id_comprobante_pagare_seq'::regclass);


--
-- Name: id_comprobante_servicio; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_comprobante_servicio ALTER COLUMN id_comprobante_servicio SET DEFAULT nextval('osiris_erp_comprobante_servicio_id_comprobante_servicio_seq'::regclass);


--
-- Name: id_convenio; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_convenios ALTER COLUMN id_convenio SET DEFAULT nextval('osiris_erp_convenios_id_convenio_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_documentos_convenio ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_documentos_convenio_id_secuencia_seq'::regclass);


--
-- Name: id_emisor; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_emisor ALTER COLUMN id_emisor SET DEFAULT nextval('osiris_erp_emisor_id_emisor_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_factura_compra_enca ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_factura_compra_enca_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_factura_deta_prov ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_factura_deta_prov_id_secuencia_seq'::regclass);


--
-- Name: id_forma_de_pago; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_forma_de_pago ALTER COLUMN id_forma_de_pago SET DEFAULT nextval('osiris_erp_forma_de_pago_id_forma_de_pago_seq'::regclass);


--
-- Name: id_abono; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_honorarios_medicos ALTER COLUMN id_abono SET DEFAULT nextval('osiris_erp_honorarios_medicos_id_abono_seq'::regclass);


--
-- Name: id_movcargos; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_movcargos ALTER COLUMN id_movcargos SET DEFAULT nextval('osiris_erp_movcargos_id_movcargos_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_movimiento_documentos ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_movimiento_documentos_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_notacredito_deta ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_notacredito_deta_id_secuencia_seq'::regclass);


--
-- Name: id_orden_compra; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_ordenes_compras_enca ALTER COLUMN id_orden_compra SET DEFAULT nextval('osiris_erp_ordenes_compras_enca_id_orden_compra_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_pases_qxurg ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_pases_qxurg_id_secuencia_seq'::regclass);


--
-- Name: id_proveedor; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_proveedores ALTER COLUMN id_proveedor SET DEFAULT nextval('osiris_erp_proveedores_id_proveedor_seq'::regclass);


--
-- Name: id_puesto; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_puestos ALTER COLUMN id_puesto SET DEFAULT nextval('osiris_erp_puestos_id_puesto_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_remision_compra_enca ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_remision_compra_enca_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_requisicion_deta ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_requisicion_deta_id_secuencia_seq'::regclass);


--
-- Name: id_requisicion; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_requisicion_enca ALTER COLUMN id_requisicion SET DEFAULT nextval('osiris_erp_requisicion_enca_id_requisicion_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_reservaciones ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_erp_reservaciones_id_secuencia_seq'::regclass);


--
-- Name: id_sucursal; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_sucursales ALTER COLUMN id_sucursal SET DEFAULT nextval('osiris_erp_sucursales_id_sucursal_seq'::regclass);


--
-- Name: id_secuancia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_tabla_ganancia ALTER COLUMN id_secuancia SET DEFAULT nextval('osiris_erp_tabla_ganancia_id_secuancia_seq'::regclass);


--
-- Name: id_tipo_comprobante; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_tipo_comprobante ALTER COLUMN id_tipo_comprobante SET DEFAULT nextval('erp_osiris_tipo_comprobante_id_tipo_comprobante_seq'::regclass);


--
-- Name: id_tipo_requisicion_compra; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_tipo_requisiciones_compra ALTER COLUMN id_tipo_requisicion_compra SET DEFAULT nextval('osiris_erp_tipo_requisiciones_co_id_tipo_requisicion_compra_seq'::regclass);


--
-- Name: id_tipo_servicio; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_erp_tipo_servicios ALTER COLUMN id_tipo_servicio SET DEFAULT nextval('osiris_erp_tipo_servicios_id_tipo_servicio_seq'::regclass);


--
-- Name: id_grupo1_producto; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_grupo1_producto ALTER COLUMN id_grupo1_producto SET DEFAULT nextval('osiris_grupo1_producto_id_grupo1_producto_seq'::regclass);


--
-- Name: id_grupo2_producto; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_grupo2_producto ALTER COLUMN id_grupo2_producto SET DEFAULT nextval('osiris_grupo2_producto_id_grupo2_producto_seq'::regclass);


--
-- Name: id_secuencia1; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_grupo_producto ALTER COLUMN id_secuencia1 SET DEFAULT nextval('osiris_grupo_producto_id_secuencia1_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_calendario_citaqx ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_calendario_citaqx_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_cirugias_deta ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_cirugias_deta_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_examenes_laboratorio ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_examenes_laboratorio_id_secuencia_seq'::regclass);


--
-- Name: id_habitacion; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_habitaciones ALTER COLUMN id_habitacion SET DEFAULT nextval('osiris_his_habitaciones_id_habitacion_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_historia_clinica ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_historia_clinica_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_historial_habitaciones ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_historial_habitaciones_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_hl7 ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_hl7_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_informacion_medica ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_information_medical_id_secuencia_seq'::regclass);


--
-- Name: id_medico; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_medicos ALTER COLUMN id_medico SET DEFAULT nextval('osiris_his_medicos_id_medico_seq'::regclass);


--
-- Name: id_linea; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_paciente ALTER COLUMN id_linea SET DEFAULT nextval('osiris_his_paciente_id_linea_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_presupuestos_deta ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_presupuestos_deta_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_resultados_imagenologia ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_resultados_imagenologia_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_resultados_laboratorio ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_resultados_laboratorio_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_solicitudes_deta ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_solicitudes_deta_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_solicitudes_labrx ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_solicitudes_labrx_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_somatometria ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_his_somatometria_id_secuencia_seq'::regclass);


--
-- Name: id_tipo_cirugia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_tipo_cirugias ALTER COLUMN id_tipo_cirugia SET DEFAULT nextval('osiris_his_tipo_cirugias_id_tipo_cirugia_seq'::regclass);


--
-- Name: id_diagnostico; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_tipo_diagnosticos ALTER COLUMN id_diagnostico SET DEFAULT nextval('osiris_his_tipo_diagnosticos_id_diagnostico_seq'::regclass);


--
-- Name: id_enfermedad; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_tipo_enfermedades ALTER COLUMN id_enfermedad SET DEFAULT nextval('osiris_his_tipo_enfermedades_id_enfermedad_seq'::regclass);


--
-- Name: id_especialidad; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_tipo_especialidad ALTER COLUMN id_especialidad SET DEFAULT nextval('osiris_his_tipo_especialidad_id_especialidad_seq'::regclass);


--
-- Name: id_tipo_paciente; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_his_tipo_pacientes ALTER COLUMN id_tipo_paciente SET DEFAULT nextval('osiris_his_tipo_pacientes_id_tipo_paciente_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_inventario_almacenes ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_inventario_almacenes_id_secuencia_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_productos ALTER COLUMN id_secuencia SET DEFAULT nextval('osiris_productos_id_secuencia_seq'::regclass);


--
-- Name: id_tipo_religion; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY osiris_tipos_religiones ALTER COLUMN id_tipo_religion SET DEFAULT nextval('osiris_tipos_religiones_id_tipo_religion_seq'::regclass);


--
-- Name: id_secuencia; Type: DEFAULT; Schema: public; Owner: admin
--

ALTER TABLE ONLY seguro_popular ALTER COLUMN id_secuencia SET DEFAULT nextval('seguro_popular_id_secuencis_seq'::regclass);


--
-- Name: emisor_id_emisor; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_emisor
    ADD CONSTRAINT emisor_id_emisor UNIQUE (id_emisor);


--
-- Name: erp_osiris_tipo_comprobante_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_tipo_comprobante
    ADD CONSTRAINT erp_osiris_tipo_comprobante_pkey PRIMARY KEY (id_tipo_comprobante);


--
-- Name: odenes_compra_enca_numero_orden_compra; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_ordenes_compras_enca
    ADD CONSTRAINT odenes_compra_enca_numero_orden_compra UNIQUE (numero_orden_compra);


--
-- Name: osiris_catalogo_productos_proveedores_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_catalogo_productos_proveedores
    ADD CONSTRAINT osiris_catalogo_productos_proveedores_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_erp_abonos_id_abono_key; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_abonos
    ADD CONSTRAINT osiris_erp_abonos_id_abono_key UNIQUE (id_abono);


--
-- Name: osiris_erp_clientes_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_clientes
    ADD CONSTRAINT osiris_erp_clientes_pkey PRIMARY KEY (id_cliente);


--
-- Name: osiris_erp_codigos_barra_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_codigos_barra
    ADD CONSTRAINT osiris_erp_codigos_barra_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_erp_comprobante_pagare_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_comprobante_pagare
    ADD CONSTRAINT osiris_erp_comprobante_pagare_pkey PRIMARY KEY (id_comprobante_pagare);


--
-- Name: osiris_erp_comprobante_servicio_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_comprobante_servicio
    ADD CONSTRAINT osiris_erp_comprobante_servicio_pkey PRIMARY KEY (id_comprobante_servicio);


--
-- Name: osiris_erp_convenios_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_convenios
    ADD CONSTRAINT osiris_erp_convenios_pkey PRIMARY KEY (id_convenio);


--
-- Name: osiris_erp_documentos_convenio_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_documentos_convenio
    ADD CONSTRAINT osiris_erp_documentos_convenio_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_erp_factura_deta_prov_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_factura_deta_prov
    ADD CONSTRAINT osiris_erp_factura_deta_prov_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_erp_honorarios_medicos_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_honorarios_medicos
    ADD CONSTRAINT osiris_erp_honorarios_medicos_pkey PRIMARY KEY (id_abono);


--
-- Name: osiris_erp_ordenes_compras_enca_id_orden_compra_key; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_ordenes_compras_enca
    ADD CONSTRAINT osiris_erp_ordenes_compras_enca_id_orden_compra_key UNIQUE (id_orden_compra);


--
-- Name: osiris_erp_pases_qxurg_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_pases_qxurg
    ADD CONSTRAINT osiris_erp_pases_qxurg_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_erp_requisicion_deta_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_requisicion_deta
    ADD CONSTRAINT osiris_erp_requisicion_deta_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_erp_sucursales_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_sucursales
    ADD CONSTRAINT osiris_erp_sucursales_pkey PRIMARY KEY (id_sucursal);


--
-- Name: osiris_erp_tabla_ganancia_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_tabla_ganancia
    ADD CONSTRAINT osiris_erp_tabla_ganancia_pkey PRIMARY KEY (id_secuancia);


--
-- Name: osiris_erp_tipo_requisiciones_compra_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_tipo_requisiciones_compra
    ADD CONSTRAINT osiris_erp_tipo_requisiciones_compra_pkey PRIMARY KEY (id_tipo_requisicion_compra);


--
-- Name: osiris_erp_tipo_servicios_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_erp_tipo_servicios
    ADD CONSTRAINT osiris_erp_tipo_servicios_pkey PRIMARY KEY (id_tipo_servicio);


--
-- Name: osiris_estados_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_estados
    ADD CONSTRAINT osiris_estados_pkey PRIMARY KEY (id_estado);


--
-- Name: osiris_grupo_producto_id_grupo_producto_key; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_grupo_producto
    ADD CONSTRAINT osiris_grupo_producto_id_grupo_producto_key UNIQUE (id_grupo_producto);


--
-- Name: osiris_his_solicitudes_deta_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_his_solicitudes_deta
    ADD CONSTRAINT osiris_his_solicitudes_deta_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_his_solicitudes_labrx_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_his_solicitudes_labrx
    ADD CONSTRAINT osiris_his_solicitudes_labrx_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_his_somatometria_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_his_somatometria
    ADD CONSTRAINT osiris_his_somatometria_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_hl7_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_his_hl7
    ADD CONSTRAINT osiris_hl7_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_information_medical_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_his_informacion_medica
    ADD CONSTRAINT osiris_information_medical_pkey PRIMARY KEY (id_secuencia);


--
-- Name: osiris_tipos_religiones_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY osiris_tipos_religiones
    ADD CONSTRAINT osiris_tipos_religiones_pkey PRIMARY KEY (id_tipo_religion);


--
-- Name: seguro_popular_pkey; Type: CONSTRAINT; Schema: public; Owner: admin; Tablespace: 
--

ALTER TABLE ONLY seguro_popular
    ADD CONSTRAINT seguro_popular_pkey PRIMARY KEY (id_secuencia);


--
-- Name: agrupacion_grupo; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX agrupacion_grupo ON osiris_grupo_producto USING btree (agrupacion);


--
-- Name: calendario_programacion_folio_de_servicio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX calendario_programacion_folio_de_servicio ON osiris_his_calendario_citaqx USING btree (folio_de_servicio);


--
-- Name: calendario_programacion_pid_paciente; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX calendario_programacion_pid_paciente ON osiris_his_calendario_citaqx USING btree (pid_paciente);


--
-- Name: comprobante_pagare_numero_comprobante; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX comprobante_pagare_numero_comprobante ON osiris_erp_comprobante_pagare USING btree (numero_comprobante_pagare);


--
-- Name: comprobante_servicio_numero_comprobante; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX comprobante_servicio_numero_comprobante ON osiris_erp_comprobante_servicio USING btree (numero_comprobante_servicio);


--
-- Name: enfermedades_id_enfermedad; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX enfermedades_id_enfermedad ON osiris_his_tipo_enfermedades USING btree (id_enfermedad);


--
-- Name: folio_de_servicio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX folio_de_servicio ON osiris_erp_cobros_enca USING btree (folio_de_servicio);

ALTER TABLE osiris_erp_cobros_enca CLUSTER ON folio_de_servicio;


--
-- Name: folio_de_servicio_abono_medico; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX folio_de_servicio_abono_medico ON osiris_erp_honorarios_medicos USING btree (folio_de_servicio);


--
-- Name: folio_de_servicio_compras_urgencia; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX folio_de_servicio_compras_urgencia ON osiris_erp_compra_farmacia USING btree (folio_de_servicio);


--
-- Name: folio_de_servicio_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX folio_de_servicio_deta ON osiris_erp_cobros_deta USING btree (folio_de_servicio);


--
-- Name: folio_de_servicio_histo_hab; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX folio_de_servicio_histo_hab ON osiris_his_historial_habitaciones USING btree (folio_de_servicio);


--
-- Name: folio_de_servicio_mov; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX folio_de_servicio_mov ON osiris_erp_movcargos USING btree (folio_de_servicio);


--
-- Name: folio_de_servicio_movdocumentos; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX folio_de_servicio_movdocumentos ON osiris_erp_movimiento_documentos USING btree (folio_de_servicio);


--
-- Name: folio_interno_dep_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX folio_interno_dep_deta ON osiris_erp_cobros_deta USING btree (folio_interno_dep);


--
-- Name: grupo_producto_agrupacion4; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX grupo_producto_agrupacion4 ON osiris_grupo_producto USING btree (agrupacion4);


--
-- Name: id_almacen; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_almacen ON osiris_almacenes USING btree (id_almacen);


--
-- Name: id_almacen_catalogo_almacen; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_almacen_catalogo_almacen ON osiris_catalogo_almacenes USING btree (id_almacen);


--
-- Name: id_almacen_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_almacen_deta ON osiris_erp_cobros_deta USING btree (id_almacen);


--
-- Name: id_almacen_inventario_almacen; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_almacen_inventario_almacen ON osiris_inventario_almacenes USING btree (id_almacen);


--
-- Name: id_almacen_solicitudes_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_almacen_solicitudes_deta ON osiris_his_solicitudes_deta USING btree (id_almacen);


--
-- Name: id_aseguradora; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_aseguradora ON osiris_aseguradoras USING btree (id_aseguradora);


--
-- Name: id_aseguradora_docconvenio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_aseguradora_docconvenio ON osiris_erp_documentos_convenio USING btree (id_aseguradora);


--
-- Name: id_aseguradora_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_aseguradora_enca ON osiris_erp_cobros_enca USING btree (id_aseguradora);


--
-- Name: id_cie_10_grupo_diagnostico; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_cie_10_grupo_diagnostico ON osiris_his_tipo_diagnosticos USING btree (id_cie_10_grupo);


--
-- Name: id_cliente_factura; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_cliente_factura ON osiris_erp_factura_enca USING btree (id_cliente);


--
-- Name: id_diagnostico_tipo_diagnostico; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_diagnostico_tipo_diagnostico ON osiris_his_tipo_diagnosticos USING btree (id_diagnostico);


--
-- Name: id_empleado_solicitudes_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_empleado_solicitudes_deta ON osiris_his_solicitudes_deta USING btree (id_empleado);


--
-- Name: id_empleados; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_empleados ON osiris_empleado USING btree (id_empleado);

ALTER TABLE osiris_empleado CLUSTER ON id_empleados;


--
-- Name: id_empresa; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_empresa ON osiris_empresas USING btree (id_empresa);

ALTER TABLE osiris_empresas CLUSTER ON id_empresa;


--
-- Name: id_empresa_cobros_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_empresa_cobros_enca ON osiris_erp_cobros_enca USING btree (id_empresa);


--
-- Name: id_empresa_docconvenios; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_empresa_docconvenios ON osiris_erp_documentos_convenio USING btree (id_empresa);


--
-- Name: id_especialidad; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_especialidad ON osiris_his_tipo_especialidad USING btree (id_especialidad);

ALTER TABLE osiris_his_tipo_especialidad CLUSTER ON id_especialidad;


--
-- Name: id_especialidad_cirugia; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_especialidad_cirugia ON osiris_his_tipo_cirugias USING btree (id_especialidad);


--
-- Name: id_estado_municipio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_estado_municipio ON osiris_municipios USING btree (id_estado);


--
-- Name: id_forma_de_pago; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_forma_de_pago ON osiris_erp_forma_de_pago USING btree (id_forma_de_pago);


--
-- Name: id_grupo1_producto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_grupo1_producto ON osiris_grupo1_producto USING btree (id_grupo1_producto);


--
-- Name: id_grupo1_producto_prod; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_grupo1_producto_prod ON osiris_productos USING btree (id_grupo1_producto);


--
-- Name: id_grupo2_producto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_grupo2_producto ON osiris_grupo2_producto USING btree (id_grupo2_producto);


--
-- Name: id_grupo2_producto_prod; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_grupo2_producto_prod ON osiris_productos USING btree (id_grupo2_producto);


--
-- Name: id_grupo_producto_prod; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_grupo_producto_prod ON osiris_productos USING btree (id_grupo_producto);


--
-- Name: id_habitacion_habitaciones; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_habitacion_habitaciones ON osiris_his_habitaciones USING btree (id_habitacion);


--
-- Name: id_habitacion_historial_habitaciones; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_habitacion_historial_habitaciones ON osiris_his_historial_habitaciones USING btree (id_habitacion);


--
-- Name: id_habitacion_mov; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_habitacion_mov ON osiris_erp_movcargos USING btree (id_habitacion_cubiculo);


--
-- Name: id_linea_paciente; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_linea_paciente ON osiris_his_paciente USING btree (id_linea);


--
-- Name: id_medico; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_medico ON osiris_his_medicos USING btree (id_medico);


--
-- Name: id_medico_compra_urgencias; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_medico_compra_urgencias ON osiris_erp_compra_farmacia USING btree (id_medico);


--
-- Name: id_medico_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_medico_enca ON osiris_erp_cobros_enca USING btree (id_medico);


--
-- Name: id_medico_honorario_med; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_medico_honorario_med ON osiris_erp_honorarios_medicos USING btree (id_medico);


--
-- Name: id_medico_osiris_his_calendario_citaqx; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_medico_osiris_his_calendario_citaqx ON osiris_his_calendario_citaqx USING btree (id_medico);


--
-- Name: id_movcargos; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_movcargos ON osiris_erp_movcargos USING btree (id_movcargos);

ALTER TABLE osiris_erp_movcargos CLUSTER ON id_movcargos;


--
-- Name: id_municipio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_municipio ON osiris_municipios USING btree (id_municipio);


--
-- Name: id_producto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_producto ON osiris_productos USING btree (id_producto);


--
-- Name: id_producto_catalogo_almacen; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_catalogo_almacen ON osiris_catalogo_almacenes USING btree (id_producto);


--
-- Name: id_producto_catalogo_proveedor; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_catalogo_proveedor ON osiris_catalogo_productos_proveedores USING btree (id_producto);


--
-- Name: id_producto_compra_urgencias; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_compra_urgencias ON osiris_erp_compra_farmacia USING btree (id_producto);


--
-- Name: id_producto_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_deta ON osiris_erp_cobros_deta USING btree (id_producto);


--
-- Name: id_producto_inventario_almacen; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_inventario_almacen ON osiris_inventario_almacenes USING btree (id_producto);


--
-- Name: id_producto_paquetes; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_paquetes ON osiris_his_cirugias_deta USING btree (id_producto);


--
-- Name: id_producto_presupuesto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_presupuesto ON osiris_his_presupuestos_deta USING btree (id_producto);


--
-- Name: id_producto_requi_oc; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_requi_oc ON osiris_erp_requisicion_deta USING btree (id_producto);


--
-- Name: id_producto_solicitudes_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_producto_solicitudes_deta ON osiris_his_solicitudes_deta USING btree (id_producto);


--
-- Name: id_proveedor; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_proveedor ON osiris_erp_proveedores USING btree (id_proveedor);


--
-- Name: id_proveedor_catalogo_productos; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_proveedor_catalogo_productos ON osiris_catalogo_productos_proveedores USING btree (id_proveedor);


--
-- Name: id_proveedor_compra_urgencias; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_proveedor_compra_urgencias ON osiris_erp_compra_farmacia USING btree (id_proveedor);


--
-- Name: id_proveedor_faccompenca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_proveedor_faccompenca ON osiris_erp_factura_compra_enca USING btree (id_proveedor);


--
-- Name: id_proveedor_facprov_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_proveedor_facprov_deta ON osiris_erp_factura_deta_prov USING btree (id_proveedor);


--
-- Name: id_proveedor_orden_compra; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_proveedor_orden_compra ON osiris_erp_ordenes_compras_enca USING btree (id_proveedor);


--
-- Name: id_proveedor_remcompenca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_proveedor_remcompenca ON osiris_erp_remision_compra_enca USING btree (id_proveedor);


--
-- Name: id_puesto_erp_puestos; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_puesto_erp_puestos ON osiris_erp_puestos USING btree (id_puesto);


--
-- Name: id_requisicion_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_requisicion_deta ON osiris_erp_requisicion_deta USING btree (id_requisicion);


--
-- Name: id_requisicion_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_requisicion_enca ON osiris_erp_requisicion_enca USING btree (id_requisicion);


--
-- Name: id_secuencia; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia ON osiris_erp_cobros_deta USING btree (id_secuencia);

ALTER TABLE osiris_erp_cobros_deta CLUSTER ON id_secuencia;


--
-- Name: id_secuencia_catalogo_almacen; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_catalogo_almacen ON osiris_catalogo_almacenes USING btree (id_secuencia);


--
-- Name: id_secuencia_compras_urgencias; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_compras_urgencias ON osiris_erp_compra_farmacia USING btree (id_secuencia);


--
-- Name: id_secuencia_deta_empleados; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_deta_empleados ON osiris_empleado_detalle USING btree (id_secuencia);


--
-- Name: id_secuencia_deta_factura; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_deta_factura ON osiris_erp_factura_deta USING btree (id_secuencia);


--
-- Name: id_secuencia_examenes_lab; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_examenes_lab ON osiris_his_examenes_laboratorio USING btree (id_secuencia);


--
-- Name: id_secuencia_factura_compra_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_factura_compra_enca ON osiris_erp_factura_compra_enca USING btree (id_secuencia);


--
-- Name: id_secuencia_his_calendario_citaqx; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_his_calendario_citaqx ON osiris_his_calendario_citaqx USING btree (id_secuencia);


--
-- Name: id_secuencia_historial_habitaciones; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_historial_habitaciones ON osiris_his_historial_habitaciones USING btree (id_secuencia);


--
-- Name: id_secuencia_inventario_almacenes; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_inventario_almacenes ON osiris_inventario_almacenes USING btree (id_secuencia);


--
-- Name: id_secuencia_movdocumentos; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_movdocumentos ON osiris_erp_movimiento_documentos USING btree (id_secuencia);


--
-- Name: id_secuencia_paquetes; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_paquetes ON osiris_his_cirugias_deta USING btree (id_secuencia);


--
-- Name: id_secuencia_presupuesto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_presupuesto ON osiris_his_presupuestos_deta USING btree (id_secuencia);


--
-- Name: id_secuencia_remision_compra_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_remision_compra_enca ON osiris_erp_remision_compra_enca USING btree (id_secuencia);


--
-- Name: id_secuencia_reservaciones; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_reservaciones ON osiris_erp_reservaciones USING btree (id_secuencia);


--
-- Name: id_secuencia_resultado_laboratorio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuencia_resultado_laboratorio ON osiris_his_resultados_laboratorio USING btree (id_secuencia);


--
-- Name: id_secuncia_historia_clinica; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_secuncia_historia_clinica ON osiris_his_historia_clinica USING btree (id_secuencia);


--
-- Name: id_subalmacen_compras_urgencias; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_subalmacen_compras_urgencias ON osiris_erp_compra_farmacia USING btree (id_subalmacen);


--
-- Name: id_tipo_admision_calendario_citaqx; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_admision_calendario_citaqx ON osiris_his_calendario_citaqx USING btree (id_tipo_paciente);


--
-- Name: id_tipo_admisiones; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_tipo_admisiones ON osiris_his_tipo_admisiones USING btree (id_tipo_admisiones);


--
-- Name: id_tipo_admisiones_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_admisiones_deta ON osiris_erp_cobros_deta USING btree (id_tipo_admisiones);


--
-- Name: id_tipo_admisiones_deta2; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_admisiones_deta2 ON osiris_erp_cobros_deta USING btree (id_tipo_admisiones2);


--
-- Name: id_tipo_admisiones_mov; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_admisiones_mov ON osiris_erp_movcargos USING btree (id_tipo_admisiones);


--
-- Name: id_tipo_admisiones_paquetes; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_admisiones_paquetes ON osiris_his_cirugias_deta USING btree (id_tipo_admisiones);


--
-- Name: id_tipo_admisiones_requi_oc; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_admisiones_requi_oc ON osiris_erp_requisicion_deta USING btree (id_tipo_admisiones);


--
-- Name: id_tipo_admuision_presupuesto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_admuision_presupuesto ON osiris_his_presupuestos_deta USING btree (id_tipo_admisiones);


--
-- Name: id_tipo_cirugia; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_tipo_cirugia ON osiris_his_tipo_cirugias USING btree (id_tipo_cirugia);

ALTER TABLE osiris_his_tipo_cirugias CLUSTER ON id_tipo_cirugia;


--
-- Name: id_tipo_cirugia_mov; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_cirugia_mov ON osiris_erp_movcargos USING btree (id_tipo_cirugia);


--
-- Name: id_tipo_cirugia_paquetes; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_cirugia_paquetes ON osiris_his_cirugias_deta USING btree (id_tipo_cirugia);


--
-- Name: id_tipo_documento_movdocumentos; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_documento_movdocumentos ON osiris_erp_movimiento_documentos USING btree (id_tipo_documento);


--
-- Name: id_tipo_paciente_calendario_citaqx; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_paciente_calendario_citaqx ON osiris_his_calendario_citaqx USING btree (id_tipo_paciente);


--
-- Name: id_tipo_paciente_mov; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX id_tipo_paciente_mov ON osiris_erp_movcargos USING btree (id_tipo_paciente);


--
-- Name: id_tipo_pacientes; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX id_tipo_pacientes ON osiris_his_tipo_pacientes USING btree (id_tipo_paciente);


--
-- Name: indice_id_presupuesto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX indice_id_presupuesto ON osiris_his_presupuestos_enca USING btree (id_presupuesto);


--
-- Name: login_empleado; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX login_empleado ON osiris_empleado USING btree (login_empleado);


--
-- Name: numero_factura_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX numero_factura_deta ON osiris_erp_factura_deta USING btree (numero_factura);


--
-- Name: numero_factura_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX numero_factura_enca ON osiris_erp_cobros_enca USING btree (numero_factura);


--
-- Name: numero_factura_enca_fact; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX numero_factura_enca_fact ON osiris_erp_factura_enca USING btree (numero_factura);


--
-- Name: numero_ntacred_factura; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX numero_ntacred_factura ON osiris_erp_factura_enca USING btree (numero_ntacred);


--
-- Name: pases_qx_urg_folio_de_servicio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pases_qx_urg_folio_de_servicio ON osiris_erp_pases_qxurg USING btree (folio_de_servicio);


--
-- Name: pases_qxurg_pid_paciente; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pases_qxurg_pid_paciente ON osiris_erp_pases_qxurg USING btree (pid_paciente);


--
-- Name: pid_paciente; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX pid_paciente ON osiris_his_paciente USING btree (pid_paciente);


--
-- Name: pid_paciente_deta; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pid_paciente_deta ON osiris_erp_cobros_deta USING btree (pid_paciente);


--
-- Name: pid_paciente_enca; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pid_paciente_enca ON osiris_erp_cobros_enca USING btree (pid_paciente);


--
-- Name: pid_paciente_hist_hab; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pid_paciente_hist_hab ON osiris_his_historial_habitaciones USING btree (pid_paciente);


--
-- Name: pid_paciente_hl7; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pid_paciente_hl7 ON osiris_his_hl7 USING btree (pid_paciente);


--
-- Name: pid_paciente_mov; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pid_paciente_mov ON osiris_erp_movcargos USING btree (pid_paciente);


--
-- Name: pid_paciente_movdocumentos; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX pid_paciente_movdocumentos ON osiris_erp_movimiento_documentos USING btree (pid_paciente);


--
-- Name: pid_pacinte_historia_clinica; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE UNIQUE INDEX pid_pacinte_historia_clinica ON osiris_his_historia_clinica USING btree (pid_paciente);


--
-- Name: reservaciones_folio_de_servicio; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX reservaciones_folio_de_servicio ON osiris_erp_reservaciones USING btree (folio_de_servicio);


--
-- Name: reservaciones_id_presupuesto; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX reservaciones_id_presupuesto ON osiris_erp_reservaciones USING btree (id_presupuesto);


--
-- Name: reservaciones_id_tipo_cirugia; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX reservaciones_id_tipo_cirugia ON osiris_erp_reservaciones USING btree (id_tipo_cirugia);


--
-- Name: rfc_paciente; Type: INDEX; Schema: public; Owner: admin; Tablespace: 
--

CREATE INDEX rfc_paciente ON osiris_his_paciente USING btree (rfc_paciente);


--
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--


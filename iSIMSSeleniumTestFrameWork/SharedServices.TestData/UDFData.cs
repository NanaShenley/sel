using SeSugar.Data;
using System;

namespace SharedServices.TestData
{
    public static class UDFData
    {
        public const string Prefix = "ManageUDF:{0}";

        public static DataPackage AddUDFFieldType(
            this DataPackage dataPackage,
            Guid? id = null,
            string code = null,
            string description = null,
            int? tenantId = null
        )
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;

            dataPackage
               .AddData("app.UDFFieldType", new
               {
                   Id = id ?? Guid.NewGuid(),
                   Code = string.Format(Prefix, code ?? "FieldType:Code"),
                   Description = string.Format(Prefix, description ?? "FieldType:Desc"),
                   DisplayOrder = 20,
                   IsVisible = 1,
                   SystemType = "String",
                   TenantId = tenantId
               });

            return dataPackage;
        }

        public static DataPackage AddUDFEntity(
            this DataPackage dataPackage,
            Guid? id = null,
            string code = null,
            string description = null,
            string entity = null,
            int? tenantId = null
        )
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;

            dataPackage
               .AddData("app.UDFEntity", new
               {
                   Id = id ?? Guid.NewGuid(),
                   Code = string.Format(Prefix, code ?? "Entity:Code"),
                   Description = string.Format(Prefix, description ?? "Entity:Desc"),
                   Entity = string.Format(Prefix, entity ?? "Entity:Ent"),
                   //Schema = string.Format(Prefix, "Schema"),
                   DisplayOrder = 20,
                   IsVisible = 1,
                   TenantId = tenantId
               });

            return dataPackage;
        }

        public static DataPackage AddUDFDomainDefinition(
            this DataPackage dataPackage,
            Guid entityID,
            Guid? id = null,
            string code = null,
            string description = null,
            string informationDomain = null,
            string securityDomain = null,
            int? tenantId = null
        )
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;

            dataPackage
               .AddData("app.UDFDomainDefinition", new
               {
                   Id = id ?? Guid.NewGuid(),
                   Code = string.Format(Prefix, code ?? "DD:Code"),
                   Description = string.Format(Prefix, description ?? "DD:Desc"),
                   InformationDomain = string.Format(Prefix, informationDomain ?? "DD:InfoDomain"),
                   SecurityDomain = string.Format(Prefix, securityDomain ?? "DD:SecurityDomain"),
                   UDFEntity = entityID,
                   DisplayOrder = 20,
                   IsVisible = 1,
                   TenantId = tenantId
               });

            return dataPackage;
        }

        public static DataPackage AddUDFDefinition(
            this DataPackage dataPackage,
            Guid udfDomainDefinitionID,
            Guid udfFieldTypeID,
            string description,
            Guid? id = null,
            bool isVisible = true,
            bool useResourceProvider = true,
            int? tenantId = null
        )
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;

            dataPackage
               .AddData("app.UDFDefinition", new
               {
                   Id = id ?? Guid.NewGuid(),
                   Description = string.Format(Prefix, description),
                   DisplayOrder = 20,
                   IsVisible = isVisible,
                   TenantId = tenantId,
                   UDFDomainDefinition = udfDomainDefinitionID,
                   UDFFieldType = udfFieldTypeID,
                   ResourceProvider = useResourceProvider ? TestSettings.TestDefaults.Default.SchoolID : null
               });

            return dataPackage;
        }

        public static DataPackage AddUDFValue(
            this DataPackage dataPackage,
            Guid udfDefinitionID,
            string value,
            Guid? id = null,
            Guid? contextID = null,
            int? tenantId = null
        )
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;

            dataPackage
               .AddData("app.UDFValue", new
               {
                   Id = id ?? Guid.NewGuid(),
                   Value = value,
                   Context = contextID ?? Guid.NewGuid(),
                   _LastChanged = DateTime.Now,
                   TenantId = tenantId,
                   UDFDefinition = udfDefinitionID
               });

            return dataPackage;
        }
        public static DataPackage AddUDFDefinitionsForTest(
            this DataPackage dataPackage,
            string visibleUDFDefinitionDescription,
            string hiddenUDFDefinitionDescription,
            string udfEntityDescription,
            int? tenantId = null,
            bool useResourceProvider = true,
            bool insertValues = false
        )
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;

            Guid udfFieldTypeID = Guid.NewGuid();
            Guid udfEntityID = Guid.NewGuid();
            Guid udfDomainDefinitionID = Guid.NewGuid();
            Guid visibleUDFDefinitionID = Guid.NewGuid();
            Guid hiddenUDFDefinitionID = Guid.NewGuid();

            dataPackage
                .AddUDFFieldType(id: udfFieldTypeID)
                    .AddUDFEntity(id: udfEntityID, entity: udfEntityDescription)
                        .AddUDFDomainDefinition(udfEntityID, id: udfDomainDefinitionID)
                            .AddUDFDefinition(udfDomainDefinitionID, udfFieldTypeID, visibleUDFDefinitionDescription, id: visibleUDFDefinitionID, useResourceProvider: useResourceProvider)
                            .AddUDFDefinition(udfDomainDefinitionID, udfFieldTypeID, hiddenUDFDefinitionDescription, id: hiddenUDFDefinitionID, isVisible: false, useResourceProvider: useResourceProvider);

            if (insertValues)
            {
                dataPackage.AddUDFValue(visibleUDFDefinitionID, "Visible:Val");
                dataPackage.AddUDFValue(hiddenUDFDefinitionID, "Hidden:Val");
            }

            return dataPackage;
        }

        public static void DeleteUDFDefinition(string description)
        {
            var sql = string.Format(
                "DELETE FROM app.UDFDefinition " +
                "WHERE [Description] = '{0}'"
                , description);

            DataAccessHelpers.Execute(sql);
        }
    }
}

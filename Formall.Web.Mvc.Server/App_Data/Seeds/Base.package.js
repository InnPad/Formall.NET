{
    data: [
        {
            "@metadata": {
                Key: "Type/00000000-0000-0000-0000-000000000000"
            },
            Name: "Type",
            Fields: {
                "Name": {
                    Type: "String"
                }
            }
        },
        {
            "@metadata": {
                Key: "Type/15d3fc60-2dd1-4a91-b46c-9052cf0243ba"
            },
            Name: "Boolean"
        },
        {
            "@metadata": {
                Key: "Type/950fc203-a3c5-4936-a8ec-29c3df037b81"
            },
            Embeddable: true,
            Name: "Text",
            Fields: {
                "en": {
                    Type: "String"
                },
                "es": {
                    Type: "String"
                },
                "jp": {
                    Type: "String"
                }
            }
        },
        {
            "@metadata": {
                Key: "Type/8347f54b-0f9b-43eb-b6b2-24fdc6eae23e"
            },
            Embeddable: true,
            Name: "Object",
            Summary: {
                "en": "Any object, no validation is performed."
            },
            Title: {
                "en": "Object",
                "es": "Objeto"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/00000000-0000-0000-0000-000000000000"
            },
            Name: "Number"
        },
        {
            "@metadata": {
                Key: "Type/String/00000000-0000-0000-0000-000000000000"
            },
            Name: "String"
        },
        {
            "@metadata": {
                Key: "Type/Item/00000000-0000-0000-0000-000000000000"
            },
            Name: "Item"
        },
        {
            "@metadata": {
                Key: "Type/Unit/00000000-0000-0000-0000-000000000000"
            },
            Name: "Unit",
        },
        {
            "@metadata": {
                Key: "Type/Model/00000000-0000-0000-0000-000000000000"
            },
            Name: "Model",
            Fields: {
                Actions: {
                    HasMany: true,
                    Type: "Action"
                },
                Fields: {
                    HasMany: true,
                    Type: "Field"
                },
                Sections: {
                    HasMany: true,
                    Type: "Section"
                },
                Summary: {
                    Type: "Text"
                },
                Title: {
                    Type: "Text"
                }
            }
        },
        {
            "@metadata": {
                Key: "Type/Table/00000000-0000-0000-0000-000000000000"
            },
            Name: "Table"
        },
        
    ]
}
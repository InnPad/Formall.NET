{
    data: [
        {
            "@metadata": {
                Key: "Type/String/00000000-0000-0000-0000-000000000000"
            },
            Name: "String",
            Title: {
                "en": "Unicode character string.",
                "es": "Cadena de caracteres unicode."
            }
        },
        {
            "@metadata": {
                Key: "Type/String/b43cfd12-c640-42cc-bfba-ff3f0ca293d9"
            },
            Name: "String/ASCII",
            Summary: {
                "en": "ASCII character string",
                "es": "Cadena de caracteres en ASCII"
            },
            Validations: {
                "AlphaNumeric": {
                    Type: "Format",
                    Format: "xxx.+xxx"
                }
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/00000000-0000-0000-0000-000000000000"
            },
            Name: "Number",
            Title: {
                "en": "Number",
                "es": "Numero"
            },
            Validations: {
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/f705f5c8-7b68-4615-b5f7-c3df3d6f1d67"
            },
            Name: "Number/Binary",
            Radix: 2,
            Title: {
                "en": "Binary",
                "es": "Binario"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/3f98b091-ff65-4b95-a2ef-30d2fa386b64"
            },
            Fields: {
                "Length": {
                },
                "Precision": {
                },
                "Scale": {
                }
            },
            Name: "Number/Decimal",
            Radix: 10,
            Title: {
                "en": "Decimal",
                "es": "Decimal"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/fcadb18f-9672-4493-bd9f-006e55c59e93"
            },
            Name: "Number/Decimal/Double",
            Length: 8,
            Precision: 16,
            Scale: 308,
            Title: {
                "en": "Double",
                "es": "Doble"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/2b2dd76a-fe95-4097-a3d8-5ecada5518e8"
            },
            Name: "Number/Decimal/Float",
            Length: 4,
            Precision: 7,
            Scale: 38,
            Title: {
                "en": "Float",
                "es": "Flotante"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/bd9e4237-1530-413e-bd3e-671ec3874199"
            },
            Name: "Number/Decimal/Int32",
            Length: 4,
            Precision: 10,
            Scale: 0,
            Title: {
                "en": "Integer 32bits",
                "es": "Entero 32bits"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/7048074b-5730-4d3a-80ba-5e57ad70ad9d"
            },
            Name: "Number/Decimal/Int16",
            Length: 2,
            Precision: 5,
            Scale: 0,
            Title: {
                "en": "Integer 16bits",
                "es": "Entero 16bits"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/07532037-3b25-40ab-81cf-b97851d91988"
            },
            Name: "Number/Hexadecimal",
            Radix: 16,
            Title: {
                "en": "Hexadecimal",
                "es": "Hexadecimal"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/a6939a1a-fa78-4c6f-969d-5c70369c9317"
            },
            Name: "Number/Octal",
            Radix: 8,
            Title: {
                "en": "Octal",
                "es": "Binario"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/223b19fa-c1b3-4467-a5e6-440b81112920"
            },
            Name: "Number/Base64",
            Radix: 64,
            Title: {
                "en": "Base 64",
                "es": "Base 64"
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/691ae285-db62-4b2e-bfb1-52562a8cbc0b"
            },
            Name: "Money",
            Title: {
                "en": "Money",
                "es": "Dinero"
            },
            Validations: {
            }
        },
        {
            "@metadata": {
                Key: "Type/Number/ad8528b8-eecd-4515-8369-32bc470dba37"
            },
            Name: "Magnitude",
            Fields: {
                "Auto": {
                    Constraint: {
                        "UnitField": true
                    },
                    Summary: {
                        "en": "Auto convert on UnitField selection change"
                    },
                    Type: "Boolean",
                },
                "Order": {
                    "Summary": {
                        en: "The order of magnitude of a number is, intuitively speaking, the number of powers of 10 contained in the number."
                    },
                    "Title": {
                        en: "Order of magnitude"
                    },
                    "Type": "OrderOfMagnitude"
                },
                "Unit": {
                    Summary: {
                        "en": "Unit type of the value. If UnitField is specified, this is the default."
                    },
                    Type: "String",
                },
                "UnitField": {
                    Summary: {
                        "en": "Name of the unit field"
                    },
                    Type: "String"
                },
                "Validations": {
                    HasMany: true,
                    Type: "Validation"
                }
            }
        },
        {
            "@metadata": {
                "Key": "Type/Item/ff63db09-dfe1-4a25-a6f7-03dedd3a3a5d"
            },
            "Name": "OrderOfMagnitude",
            "Fields": {
                "Order": {
                    "Type": "SByte",
                    "Title": {
                        "Type": "Text"
                    }
                },
                "LongScale": {
                    "Type": "$/Type/Text"
                },
                "Prefix": {
                    "Type": "$/Type/String/ASCII"
                },
                "ShortScale": {
                    "Type": "$/Type/Text"
                },
                "Symbol": {
                    "Type": "$/Type/String/ASCII"
                }
            },
            "Title": {
                "en": "Order of magnitude"
            }
        }
    ]
}
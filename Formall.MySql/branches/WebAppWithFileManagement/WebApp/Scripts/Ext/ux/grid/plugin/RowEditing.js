/**
* @class Ext.ux.grid.plugin.RowEditing
* @extends Ext.grid.plugin.RowEditing
* 
* The Initial Developer of the Original Code is tz (atian25@qq.com)
* @see http://www.sencha.com/forum/showthread.php?131482-Ext.ux.RowEditing-add-some-usefull-features
* 
* @author Harald Hanek (c) 2011-2012
* @license http://harrydeluxe.mit-license.org
*/

Ext.define('Ext.ux.grid.plugin.RowEditing', {
    extend: 'Ext.grid.plugin.RowEditing',
    alias: 'plugin.ux.rowediting',

    removePhantomsOnCancel: true,

    init: function (grid) {
        var me = this;
        me.addEvents('canceledit');
        me.callParent(arguments);
        grid.addEvents('canceledit');
        grid.relayEvents(me, ['canceledit']);
    },

    /**
    *      @example
    *      {header:'',dataIndex:'phone',fieldType:'numberfield',field:{allowBlank:true}}
    * provide default field config
    * @param {String} fieldType: numberfield, checkboxfield, passwordField
    * @return {Object} 
    * @protected
    */
    getFieldCfg: function (fieldType) {
        switch (fieldType) {
            case 'passwordField':
                return {
                    xtype: 'textfield',
                    inputType: 'password',
                    allowBlank: false
                }
            case 'numberfield':
                return {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowBlank: false
                }

            case 'checkboxfield':
                return {
                    xtype: 'checkboxfield',
                    inputValue: 'true',
                    uncheckedValue: 'false'
                }
        }
    },

    /**
    * Help to config field,just giving a fieldType and field as additional cfg.
    * see {@link #getFieldCfg}
    * @private
    * @override
    */
    getEditor: function () {
        var me = this;


        if (!me.editor) {
            Ext.each(me.grid.headerCt.getGridColumns(), function (item, index, allItems) {
                if (item.fieldType) {
                    item.field = Ext.applyIf(item.field || {}, this.getFieldCfg(item.fieldType))
                }
            }, this)
            // keep a reference for custom editor..
            me.editor = me.initEditor();
        }
        me.editor.editingPlugin = me
        return me.editor;
    },

    /**
    * if clicksToEdit===0 then mun the click/dblclick event
    * @private
    * @override
    */
    initEditTriggers: function () {
        var me = this
        var clickEvent = me.clicksToEdit === 1 ? 'click' : 'dblclick'
        me.callParent(arguments);
        if (me.clicksToEdit === 0) {
            me.mun(me.view, 'cell' + clickEvent, me.startEditByClick, me);
        }
    },

    /**
    * add a record and start edit it
    * 
    * @param {Object} data Data to initialize the Model's fields with
    * @param {Number} position The position where the record will added. -1
    *            will be added record at last position.
    */
    startAdd: function (data, position) {
        var me = this;

        /*var cfg = Ext.apply({
        addInPlace: this.addInPlace,
        addPosition: this.addPosition,
        colIndex: 0
        }, config)*/

        var record = me.grid.store.model.create(data); //var record = data.isModel ? data : me.grid.store.model.create(data);
        record._blank = true;

        position = (position && position != -1 && parseInt(position + 1) <= parseInt(me.grid.store.getCount())) ? position
				: (position == -1) ? parseInt(me.grid.store.getCount()) : 0;

        //find the position
        /*if (cfg.addInPlace) {
        var selected = me.grid.getSelectionModel().getSelection()
        if (selected && selected.length > 0) {
        position = me.grid.store.indexOf(selected[0])
        console.log('a', position)
        position += (cfg.addPosition <= 0) ? 0 : 1
        } else {
        position = 0
        }
        } else {
        position = (cfg.addPosition == -1 ? me.grid.store.getCount() : cfg.addPosition) || 0
        }*/

        var autoSync = me.grid.store.autoSync;
        me.grid.store.autoSync = false;
        me.grid.store.insert(position, record);
        me.grid.store.autoSync = autoSync;
        me.adding = true;
        me.startEdit(position, 0); //me.startEdit(position, cfg.colIndex);

        if (me.editor.floatingButtons && me.editor.form.isValid()) {
            me.editor.floatingButtons.child('#update').setDisabled(false);
        }

        //since autoCancel:true dont work for me
        /*if (me.hideTooltipOnAdd && me.getEditor().hideToolTip) {
        me.getEditor().hideToolTip()
        }*/
    },

    /**
    * Modify: if is editing, cancel first.
    * @private
    * @override
    */
    startEdit: function (record, columnHeader) {
        var me = this;
        if (me.editing) {
            me.cancelEdit();
        }
        me.callParent(arguments);
    },

    startEditByClick: function () {
        var me = this;

        if (!me.editing || me.clicksToMoveEditor === me.clicksToEdit) {
            if (me.context && me.context.record._blank)
                me.cancelEdit();

            me.callParent(arguments);
        }
    },

    /**
    * Modify: set adding=false
    * @private
    * @override
    */
    completeEdit: function () {
        var me = this;
        if (me.editing && me.validateEdit()) {
            me.editing = false;
            me.fireEvent('edit', me.context);
        }
        me.adding = false
    },

    moveEditorByClick: function () {
        var me = this;
        if (me.editing) {
            if (me.context && me.context.record._blank)
                me.cancelEdit();

            me.editing = false;
            me.superclass.onCellClick.apply(me, arguments);
        }
    },

    cancelEdit: function () {
        var me = this;
        if (me.editing) {
            me.getEditor().cancelEdit();
            me.callParent(arguments); //me.editing = false;
            me.fireEvent('canceledit', me.context);

            if (me.removePhantomsOnCancel/*me.autoRecoverOnCancel*/) {
                if (me.context.record._blank && me.context.record.store/*me.adding*/) {
                    me.context.store.remove(me.context.record);
                    //me.adding = false
                }
                else {
                    me.context.record.reject();
                }
            }
        }
    }
});

if (Ext.grid.RowEditor) {
    Ext.apply(Ext.grid.RowEditor.prototype, {
        saveBtnText: 'Update',
        cancelBtnText: 'Cancel',
        errorsText: 'Error',
        dirtyText: 'Dirty'
    });
}
/*

This file is part of Ext JS 4

Copyright (c) 2011 Sencha Inc

Contact:  http://www.sencha.com/contact

GNU General Public License Usage
This file may be used under the terms of the GNU General Public License version 3.0 as published by the Free Software Foundation and appearing in the file LICENSE included in the packaging of this file.  Please review the following information to ensure the GNU General Public License version 3.0 requirements will be met: http://www.gnu.org/copyleft/gpl.html.

If you are unsure which license is appropriate for your use, please contact the sales department at http://www.sencha.com/contact.

*/
/**
* @class Ext.ux.DataView.LabelEditor
* @extends Ext.Editor
*/
Ext.define('Ext.ux.DataView.LabelEditor', {

    extend: 'Ext.Editor',

    alignment: 'tl-tl',

    cls: 'x-small-editor',

    hideEl: false,

    ignoreNoChange: true,

    completeOnEnter: true,

    cancelOnEsc: true,

    shim: false,

    autoSize: {
        width: 'boundEl',
        height: 'field'
    },

    labelSelector: 'x-editable',//'span.x-editable',

    requires: [
        'Ext.form.field.Text'
    ],

    constructor: function (config) {
        config.field = config.field || Ext.create('Ext.form.field.Text', {
            allowBlank: false,
            growMin: 90,
            growMax: 240,
            grow: true,
            selectOnFocus: true,
            vtype: 'filename'
        });
        this.callParent([config]);
    }, 

    init: function (view) {
        this.view = view;
        this.mon(view, 'render', this.bindEvents, this);
        this.on('complete', this.onSave, this);
    },

    // initialize events
    bindEvents: function () {
        this.mon(this.view.getEl(), {
            click: Ext.Function.bind(this.onClick, this)
        });
        /*this.view.on({
            scope: this,
            containerclick: this.doBlur,
            click: this.doBlur
        });
        this.view.getEl().on('mousedown', this.onMouseDown, this, { delegate: this.labelSelector });*/
    },

    doBlur: function () {
        if (this.editing) {
            this.field.blur();
        }
    },

    /*onMouseDown: function (evt, target) {
        if (!evt.ctrlKey && !evt.shiftKey) {
            var item, record;
            item = this.view.findItemFromChild(target);
            evt.stopEvent();
            record = this.view.store.getAt(this.view.indexOf(item));
            this.startEdit(target, record.data[this.dataIndex]);
            this.activeRecord = record;
        } else {
            evt.preventDefault();
        }
    },*/

    // on mousedown show editor
    onClick: function (e, target) {
        var me = this,
            item, record;

        if (Ext.fly(target).hasCls(me.labelSelector) && !me.editing && !e.ctrlKey && !e.shiftKey) {
            e.stopEvent();
            item = me.view.findItemByChild(target);
            record = me.view.store.getAt(me.view.indexOf(item));
            me.startEdit(target, record.data[me.dataIndex]);
            me.activeRecord = record;
        } else if (me.editing) {
            me.field.blur();
            e.preventDefault();
        }
    },

    // update record
    onSave: function (ed, value) {
        this.activeRecord.set(this.dataIndex, value);
    }
});
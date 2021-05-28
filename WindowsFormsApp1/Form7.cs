﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/mm";
            dateTimePicker1.ShowUpDown = true;
            customerTableAdapter1.Fill(fullDatabase1.Customer);
            paymentTableAdapter1.Fill(fullDatabase1.Payment);
            bookingSummaryTableAdapter1.Fill(fullDatabase1.BookingSummary);
            label7.Text += getAmountDue();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(paymentDetailIsValid())
            {
                paymentTableAdapter1.Insert(DateTime.Now, getAmountDue(), currentUser.getSummaryID(), "EFT");
                updateBookingStatus();
                label8.Text += getAmountDue();
                label8.Visible = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

       
        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private string getSurname()
        {
            int startIndex = 0;
            for (int i = 0; i < textBox3.Text.Length; i++)
            {
                if (textBox3.Text[i] == ' ')
                {
                    startIndex = i + 1;
                    break;
                }
            }
            return textBox3.Text.Substring(startIndex);
        }
        private bool nameIsValid()
        {
            for (int i = 0; i < fullDatabase1.Customer.Rows.Count; i++)
            {
                if (fullDatabase1.Tables["Customer"].Rows[i]["emailID"].ToString() == currentUser.getEmailID() && fullDatabase1.Tables["Customer"].Rows[i]["surname"].ToString() == getSurname())
                    return true;
            }
            return false;
        }
        private bool cardNumberIsValid()
        {
            return textBox1.Text.Length == 16;
        }
        private bool cardNotExpired()
        {
            return dateTimePicker1.Value.Year > DateTime.Today.Year;
        }
        private bool cvvIsValid()
        {
            return textBox2.Text.Length == 3;
        }
        private bool paymentDetailIsValid()
        {
            int count = 0;
            if(!nameIsValid())
            {
                textBox3.BackColor = Color.Red;
                count++;
            }
            if(!cardNumberIsValid())
            {
                textBox1.BackColor = Color.Red;
                count++;
            }
            if(!cvvIsValid())
            {
                textBox2.BackColor = Color.Red;
                count++;
            }
            if(!cardNotExpired())
            {
                label6.Visible = true;
                count++;
            }
            return count == 0;
        }
        private string getAmountDue()
        {
            for (int i = 0; i < fullDatabase1.BookingSummary.Rows.Count; i++)
            {
                if(fullDatabase1.Tables["BookingSummary"].Rows[i]["summaryID"].ToString() == currentUser.getSummaryID()+"")
                    return fullDatabase1.Tables["BookingSummary"].Rows[i]["amountDue"].ToString();
            }
            return "";
        }
        private void updateBookingStatus()
        {
            for (int i = 0; i < fullDatabase1.BookingSummary.Rows.Count; i++)
            {
                if (fullDatabase1.Tables["BookingSummary"].Rows[i]["summaryID"].ToString() == currentUser.getSummaryID() + "")
                {
                    fullDatabase1.Tables["BookingSummary"].Rows[i]["bookingStatus"] = "Complete";
                    bookingSummaryTableAdapter1.Update(fullDatabase1.BookingSummary);
                }

            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }

}
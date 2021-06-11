﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class adminForm : Form
    {
        public adminForm()
        {
            InitializeComponent();
            bookingInnerTa.Fill(fullDs.BookingInner);
            paymentTa.Fill(fullDs.Payment);
            bookingSummaryTa.Fill(fullDs.BookingSummary);
            viewBookingInnerTa.Fill(fullDs.viewBookingInner);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(signUIsValid())
            {
                customerTa.Insert(capFirst(emailTextBox.Text), capFirst(firtNameTextBox.Text), capFirst(lastNameTextBox.Text), IDTextBox.Text, cellNumberTextBox.Text, "00000000", capFirst(addressLine1TextBox.Text), capFirst(addressLine2TextBox.Text), capFirst(cityTextBox.Text), postalCodeTextBox.Text);
                label9.Visible = true;
                panel1.Enabled = false;
            }
        }
        private string capFirst(string s)
        {
            return (s[0] + "").ToUpper() + s.Substring(1).ToLower();
        }
        private bool isDigit(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return true;
            return false;
        }
        private bool isAllDigit(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!isDigit(s[i]))
                    return false;
            }
            return true;
        }
        private bool isLetter(char c)
        {
            if (c >= 'a' && c <= 'z')
                return true;
            return false;
        }
        private bool isAllLetters(string s)
        {
            s = s.ToLower();
            for (int i = 0; i < s.Length; i++)
            {
                if (!isLetter(s[i]))
                    return false;
            }
            return true;
        }
        private bool NameIsValid()
        {
            int count = 0;
            if (!isAllLetters(firtNameTextBox.Text) || firtNameTextBox.Text.Length < 3)
            {
                firtNameTextBox.BackColor = Color.Red;
                count++;
            }
            if (!isAllLetters(lastNameTextBox.Text) || lastNameTextBox.Text.Length < 3)
            {
                lastNameTextBox.BackColor = Color.Red;
                count++;
            }
            return count == 0;
        }
        private bool CellNumberisValid()
        {
            if (!isAllDigit(cellNumberTextBox.Text) || cellNumberTextBox.Text.Length != 10)
            {
                cellNumberTextBox.BackColor = Color.Red;
                return false;
            }
            return true;
        }
        private bool EmailISValid()
        {
            if (emailTextBox.Text != null)
            {
                EmailAddressAttribute email = new EmailAddressAttribute();
                if (!email.IsValid(emailTextBox.Text))
                {
                    emailTextBox.BackColor = Color.Red;
                    label22.Visible = true;
                    return false;
                }
                if (EmailIsRegistred())
                {
                    emailTextBox.BackColor = Color.Red;
                    label23.Visible = true;
                    return false;
                }
                return true;
            }
            emailTextBox.BackColor = Color.Red;
            return false;
        }
        private bool EmailIsRegistred()
        {
            for (int i = 0; i < fullDs.Customer.Rows.Count; i++)
            {
                if (fullDs.Customer[i].emailID.Equals(emailTextBox.Text, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        private bool AddrressIsValid()
        {
            if (!isAllDigit(postalCodeTextBox.Text) || postalCodeTextBox.Text.Length != 4)
            {
                postalCodeTextBox.BackColor = Color.Red;
                return false;
            }
            return true;
        }
        private bool IdIsValid()
        {
            if (!isAllDigit(IDTextBox.Text) || IDTextBox.Text.Length != 13)
            {
                IDTextBox.BackColor = Color.Red;
                return false;
            }
            return true;
        }
        private bool signUIsValid()
        {
            int count = 0;
            if (!NameIsValid())
                count++;
            if (!AddrressIsValid())
                count++;
            if (!CellNumberisValid())
                count++;
            if (!EmailISValid())
                count++;
            if (!IdIsValid())
                count++;
            return count == 0;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bookingInnerTa.FillByPreference(fullDs.BookingInner,textBox1.Text);
        }

        private void bookingInnerDGV_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            label4.Visible = false;
            DataRow dataRow = fullDs1.BookingInner.NewRow();
            for(int i = 0; i < dataRow.ItemArray.Length; i++)
            {
                dataRow[i] = bookingInnerDGV.CurrentRow.Cells[i].Value;
            }
            try
            {
                fullDs1.BookingInner.Rows.Add(dataRow);
                label5.Visible = false;
            }
            catch(ConstraintException)
            {
                label5.Visible = true;
            }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView1.Rows.Count;i++)
            {
                cancelBooking((int)dataGridView1.CurrentRow.Cells[4].Value);
            }
            label4.Visible = true;
            bookingInnerTa.Fill(fullDs.BookingInner);
            fullDs1.Clear();
        }
        private string calculateAmountDue(string s)
        {
            s = s.Substring(2, s.Length - 5);
            double due = double.Parse(s) * 0.5;
            return due + "";
        }
        private void processRefund(int summaryID)
        {
            for (int i = 0; i < fullDs.Payment.Rows.Count; i++)
            {
                if (fullDs.Payment[i].summaryID == summaryID)
                {
                    string newAmount = calculateAmountDue(fullDs.Payment[i].amountDue.ToString());
                    paymentTa.Insert(DateTime.Today, "-R " + newAmount, summaryID, fullDs.Payment[i].typeOfPayment);
                    paymentTa.Fill(fullDs.Payment);
                    break;
                }
            }
        }
        private void cancelBooking(int summaryID)
        {
            for (int i = 0; i < fullDs.BookingSummary.Rows.Count; i++)
            {
                if (fullDs.Payment[i].summaryID == summaryID)
                {
                    fullDs.BookingSummary[i].bookingStatus = "Cancelled";
                    bookingSummaryTa.Update(fullDs.BookingSummary);
                    bookingSummaryTa.Fill(fullDs.BookingSummary);
                    bookingInnerTa.Fill(fullDs.BookingInner);
                    processRefund(summaryID);
                }
            }
        }

        private void firtNameTextBox_Enter(object sender, EventArgs e)
        {
            if (firtNameTextBox.Text == "First name")
            {
                firtNameTextBox.Text = null;
                firtNameTextBox.ForeColor = Color.Black;
            }
        }

        private void firtNameTextBox_Leave(object sender, EventArgs e)
        {
            if (firtNameTextBox.Text == "")
            {
                firtNameTextBox.Text = "First name";
                firtNameTextBox.ForeColor = Color.Gray;
            }
        }

        private void lastNameTextBox_Enter(object sender, EventArgs e)
        {
            if (lastNameTextBox.Text == "Last name")
            {
                lastNameTextBox.Text = null;
                lastNameTextBox.ForeColor = Color.Black;
            }
        }

        private void lastNameTextBox_Leave(object sender, EventArgs e)
        {
            if (lastNameTextBox.Text == "")
            {
                lastNameTextBox.Text = "Last name";
                lastNameTextBox.ForeColor = Color.Gray;
            }
        }

        private void addressLine1TextBox_Enter(object sender, EventArgs e)
        {
            if (addressLine1TextBox.Text == "Street Address Line 1")
            {
                addressLine1TextBox.Text = null;
                addressLine1TextBox.ForeColor = Color.Black;
            }
        }

        private void addressLine1TextBox_Leave(object sender, EventArgs e)
        {
            if (addressLine1TextBox.Text == "")
            {
                addressLine1TextBox.Text = "Street Address Line 1";
                addressLine1TextBox.ForeColor = Color.Gray;
            }
        }

        private void addressLine2TextBox_Enter(object sender, EventArgs e)
        {
            if (addressLine2TextBox.Text == "Street Address Line 2")
            {
                addressLine2TextBox.Text = null;
                addressLine2TextBox.ForeColor = Color.Black;
            }
        }

        private void addressLine2TextBox_Leave(object sender, EventArgs e)
        {
            if (addressLine2TextBox.Text == "")
            {
                addressLine2TextBox.Text = "Street Address Line 2";
                addressLine2TextBox.ForeColor = Color.Gray;
            }
        }

        private void cityTextBox_Enter(object sender, EventArgs e)
        {
            if (cityTextBox.Text == "City")
            {
                cityTextBox.Text = null;
                cityTextBox.ForeColor = Color.Black;
            }
        }

        private void cityTextBox_Leave(object sender, EventArgs e)
        {
            if (cityTextBox.Text == "")
            {
                cityTextBox.Text = "City";
                cityTextBox.ForeColor = Color.Gray;
            }
        }

        private void postalCodeTextBox_Enter(object sender, EventArgs e)
        {
            if (postalCodeTextBox.Text == "Postal code")
            {
                postalCodeTextBox.Text = null;
                postalCodeTextBox.ForeColor = Color.Black;
            }
        }

        private void postalCodeTextBox_Leave(object sender, EventArgs e)
        {
            if (postalCodeTextBox.Text == "")
            {
                postalCodeTextBox.Text = "Postal code";
                postalCodeTextBox.ForeColor = Color.Gray;
            }
        }

        private void IDTextBox_Enter(object sender, EventArgs e)
        {
            if (IDTextBox.Text == "ID number")
            {
                IDTextBox.Text = null;
                IDTextBox.ForeColor = Color.Black;
            }
        }

        private void IDTextBox_Leave(object sender, EventArgs e)
        {
            if (IDTextBox.Text == "")
            {
                IDTextBox.Text = "ID number";
                IDTextBox.ForeColor = Color.Gray;
            }
        }

        private void cellNumberTextBox_Enter(object sender, EventArgs e)
        {
            if (cellNumberTextBox.Text == "Cell number")
            {
                cellNumberTextBox.Text = null;
                cellNumberTextBox.ForeColor = Color.Black;
            }
        }

        private void cellNumberTextBox_Leave(object sender, EventArgs e)
        {
            if (cellNumberTextBox.Text == "")
            {
                cellNumberTextBox.Text = "Cell number";
                cellNumberTextBox.ForeColor = Color.Gray;
            }
        }

        private void emailTextBox_Enter(object sender, EventArgs e)
        {
            if (emailTextBox.Text == "Email")
            {
                emailTextBox.Text = null;
                emailTextBox.ForeColor = Color.Black;
            }
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            if (emailTextBox.Text == "")
            {
                emailTextBox.Text = "Email";
                emailTextBox.ForeColor = Color.Gray;
            }
        }

        private void firtNameTextBox_TextChanged(object sender, EventArgs e)
        {
            firtNameTextBox.BackColor = Color.White;
        }

        private void lastNameTextBox_TextChanged(object sender, EventArgs e)
        {
            lastNameTextBox.BackColor = Color.White;
        }

        private void addressLine1TextBox_TextChanged(object sender, EventArgs e)
        {
            addressLine1TextBox.BackColor = Color.White;
        }

        private void addressLine2TextBox_TextChanged(object sender, EventArgs e)
        {
            addressLine2TextBox.BackColor = Color.White;
        }

        private void cityTextBox_TextChanged(object sender, EventArgs e)
        {
            cityTextBox.BackColor = Color.White;
        }

        private void postalCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            postalCodeTextBox.BackColor = Color.White;
        }

        private void IDTextBox_TextChanged(object sender, EventArgs e)
        {
            IDTextBox.BackColor = Color.White;
        }

        private void cellNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            cellNumberTextBox.BackColor = Color.White;
        }

        private void emailTextBox_TextChanged(object sender, EventArgs e)
        {
            emailTextBox.BackColor = Color.White;
            label23.Visible = false;
            label22.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            viewBookingInnerTa.FillByPreference(fullDs.viewBookingInner, textBox2.Text);
        }
    }
}
